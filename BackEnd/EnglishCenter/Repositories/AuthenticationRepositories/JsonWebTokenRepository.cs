using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EnglishCenter.Models;
using EnglishCenter.Repositories.IRepositories;
using Microsoft.IdentityModel.Tokens;

namespace EnglishCenter.Repositories.AuthenticationRepositories
{
    public class JsonWebTokenRepository : IJsonWebTokenRepository
    {
        private readonly IClaimRepository _claimRepo;
        private readonly IConfiguration _config;

        public JsonWebTokenRepository(IClaimRepository claimRepo, IConfiguration config) 
        {
            _claimRepo = claimRepo;
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

        public async Task<string> GenerateUserTokenAsync(User user, DateTime expireDate)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var claims = await _claimRepo.GetClaims(user);
            var authenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Secret"]!));

            claims.Add(new Claim(ClaimTypes.Name, user.UserName));
            claims.Add(new Claim("Id", user.Id));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

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
    }
}
