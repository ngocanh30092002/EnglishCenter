using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using EnglishCenter.Business.IServices;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.Presentation.Global;
using EnglishCenter.Presentation.Global.Enum;
using EnglishCenter.Presentation.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace EnglishCenter.Business.Services.Authorization
{
    public class JsonTokenService : IJsonTokenService
    {
        private readonly UserManager<User> _userManager;
        private readonly IClaimService _claimService;
        private readonly IConfiguration _config;

        public JsonTokenService(UserManager<User> userManager, IClaimService claimService, IConfiguration config)
        {
            _userManager = userManager;
            _claimService = claimService;
            _config = config;
        }
        public async Task<DecodeJwtToken> DecodeToken(string token)
        {
            if (string.IsNullOrEmpty(token))
                return null;

            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var securityToken = jwtTokenHandler.ReadJwtToken(token);

            var decodeJwtTokend = new DecodeJwtToken()
            {
                KeyId = securityToken.Id,
                Audience = securityToken.Audiences.ToList(),
                Claims = securityToken.Claims.ToList(),
                ExpireDate = securityToken.ValidTo,
                ValidFrom = securityToken.ValidFrom,
                Header = securityToken.EncodedHeader,
                Payload = securityToken.EncodedPayload,
                Issuer = securityToken.Issuer,
                SigningAlgorithm = securityToken.SignatureAlgorithm,
                RawData = securityToken.RawData,
                Subject = securityToken.Subject,
            };

            return decodeJwtTokend;
        }

        public async Task<string> GenerateUserTokenAsync(User user, DateTime expireDate, Provider provider = 0)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var claims = await _claimService.GetClaimsUserAsync(user);
            var authenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Secret"]!));

            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
            claims.Add(new Claim("Provider", provider.ToString()));

            var tokenDes = new SecurityTokenDescriptor()
            {
                Issuer = _config["JWT:ValidIssuer"]!,
                Audience = _config["JWT:ValidAudience"]!,
                Subject = new ClaimsIdentity(claims),
                Expires = expireDate,
                SigningCredentials = new SigningCredentials(authenKey, SecurityAlgorithms.HmacSha512)
            };

            var securityToken = jwtTokenHandler.CreateToken(tokenDes);

            return jwtTokenHandler.WriteToken(securityToken);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public async Task<string> GetRefreshTokenFromUser(User user, Provider provider = Provider.System)
        {
            var userToken = await _userManager.GetAuthenticationTokenAsync(user, provider.ToString(), GlobalVariable.REFRESH_TOKEN);
            return userToken;
        }

        public async Task<Response> RenewTokenAsync(string accessToken, string refreshToken)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var secretKeyBytes = Encoding.UTF8.GetBytes(_config["JWT:Secret"]!);
            var tokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidAudience = _config["JWT:ValidAudience"],
                ValidIssuer = _config["JWT:ValidIssuer"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Secret"]!)),
                ValidateLifetime = false
            };

            try
            {
                var tokenInVerification = jwtTokenHandler.ValidateToken(accessToken, tokenValidationParameters, out var validatedToken);

                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var isCorrectAlg = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512, StringComparison.InvariantCultureIgnoreCase);

                    if (!isCorrectAlg)
                    {
                        return new Response()
                        {
                            Success = false,
                            Message = "Invalid Token",
                            StatusCode = System.Net.HttpStatusCode.Unauthorized
                        };
                    }

                    var utcExpireSeconds = long.Parse(tokenInVerification.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp).Value);
                    DateTime utcDateTime = DateTimeOffset.FromUnixTimeSeconds(utcExpireSeconds).UtcDateTime;

                    if (utcDateTime > DateTime.UtcNow)
                    {
                        return new Response()
                        {
                            Success = false,
                            Message = "Invalid Token",
                            StatusCode = System.Net.HttpStatusCode.Unauthorized
                        };
                    }

                    var userId = tokenInVerification.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                    var user = await _userManager.FindByIdAsync(userId);
                    var provider = tokenInVerification.Claims.FirstOrDefault(c => c.Type == "Provider");
                    var providerValue = provider == null ? Provider.System : (Provider)Enum.Parse(typeof(Provider), provider.Value);

                    var storedToken = await _userManager.GetAuthenticationTokenAsync(user, providerValue.ToString(), GlobalVariable.REFRESH_TOKEN);
                    if (storedToken != null && storedToken == refreshToken)
                    {
                        var newRefreshToken = GenerateRefreshToken();
                        var result = await _userManager.SetAuthenticationTokenAsync(user, providerValue.ToString(), GlobalVariable.REFRESH_TOKEN, newRefreshToken);

                        if (result.Succeeded)
                        {
                            return new Response()
                            {
                                Success = true,
                                Token = await GenerateUserTokenAsync(user, DateTime.UtcNow.AddMinutes(GlobalVariable.TOKEN_EXPIRED), providerValue),
                                RefreshToken = newRefreshToken,
                                StatusCode = System.Net.HttpStatusCode.OK
                            };
                        }
                    }

                    return new Response()
                    {
                        Success = false,
                        Message = "Invalid Token",
                        StatusCode = System.Net.HttpStatusCode.Unauthorized
                    };
                }

                return new Response()
                {
                    Success = false,
                    Message = "Invalid Token",
                    StatusCode = System.Net.HttpStatusCode.Unauthorized
                };
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);

                return new Response()
                {
                    Success = false,
                    Message = ex.Message,
                    StatusCode = System.Net.HttpStatusCode.InternalServerError
                }; ;
            }
        }

        public bool VerifyAccessToken(string accessToken)
        {
            try
            {
                var jwtTokenHandler = new JwtSecurityTokenHandler();
                var secretKeyBytes = Encoding.UTF8.GetBytes(_config["JWT:Secret"]!);
                var tokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidAudience = _config["JWT:ValidAudience"],
                    ValidIssuer = _config["JWT:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Secret"]!)),
                    ValidateLifetime = false
                };

                var tokenInVerification = jwtTokenHandler.ValidateToken(accessToken, tokenValidationParameters, out _);

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return false;
            }
        }
    }
}
