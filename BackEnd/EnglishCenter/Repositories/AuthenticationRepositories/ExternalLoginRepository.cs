using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text.Json;
using System.Web;
using EnglishCenter.Database;
using EnglishCenter.Global;
using EnglishCenter.Global.Enum;
using EnglishCenter.Models;
using EnglishCenter.Repositories.IRepositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.DotNet.Scaffolding.Shared;
using Newtonsoft.Json;

namespace EnglishCenter.Repositories.AuthenticationRepositories
{
    public class ExternalLoginRepository : IExternalLoginRepository
    {
        private readonly IJsonWebTokenRepository _jwtRepo;
        private readonly SignInManager<User> _signInManage;
        private readonly UserManager<User> _userManager;
        private readonly EnglishCenterContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IAccountRepository _accountRepo;

        public ExternalLoginRepository(
            IJsonWebTokenRepository jwtRepo,
            SignInManager<User> signInManager,
            UserManager<User> userManager,
            EnglishCenterContext context,
            RoleManager<IdentityRole> roleManager,
            IAccountRepository accountRepo) 
        {
            _jwtRepo = jwtRepo;
            _signInManage = signInManager;
            _userManager = userManager;
            _context = context;
            _roleManager = roleManager;
            _accountRepo = accountRepo;
        }
        public async Task<Response> SignInGoogleAsync(Dictionary<string, string> requestParams, string googleTokenUrl)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var requestContent = new FormUrlEncodedContent(requestParams);
                    var response = await client.PostAsync(googleTokenUrl, requestContent);
                    var responseContent = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        var tokenResponse = JsonConvert.DeserializeObject<TokenReponse>(responseContent);
                        return await SignInAndRegisterForUser(tokenResponse, Provider.Google.ToString());
                    }
                }

                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.InternalServerError,
                    Success = false,
                    Message = "Login with google account failed",
                    RedirectLink = GlobalVariable.CLIENT_URL + "login"
                };
            }
            catch (Exception ex)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.InternalServerError,
                    Success = false,
                    Message = ex.Message,
                    RedirectLink = GlobalVariable.CLIENT_URL + "login"
                };
            }
        }

        private async Task<Response> SignInAndRegisterForUser(TokenReponse tokenResponse, string provider)
        {
            var tokenInfo = await _jwtRepo.DecodeToken(tokenResponse.Token_Id);

            var result = await _signInManage.ExternalLoginSignInAsync(provider, tokenInfo.Subject, isPersistent: false, bypassTwoFactor: false);

            if(result.Succeeded)
            {
                var userInfo = await _userManager.FindByLoginAsync(provider, tokenInfo.Subject);
                var token = await _jwtRepo.GenerateUserTokenAsync(userInfo, DateTime.UtcNow.AddMinutes(GlobalVariable.TOKEN_EXPIRED), Provider.Google);
                var userToken = await _jwtRepo.GetRefreshTokenFromUser(userInfo, Global.Enum.Provider.Google);

                if (string.IsNullOrEmpty(userToken))
                {
                    userToken = _jwtRepo.GenerateRefreshToken();
                    await _userManager.SetAuthenticationTokenAsync(userInfo, Provider.Google.ToString(), GlobalVariable.REFRESH_TOKEN, userToken);
                }

                return new Response()
                {
                    RedirectLink = GlobalVariable.CLIENT_URL,
                    Message = "",
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Success = true,
                    Token = token,
                    RefreshToken = userToken
                };
            }

            if (result.IsLockedOut)
            {
                return new Response()
                {
                    RedirectLink = GlobalVariable.CLIENT_URL + "account/lockout",
                    Message = "The account is currently locked",
                    StatusCode = System.Net.HttpStatusCode.Redirect,
                    Success = true
                };
            }
            else
            {
                // Account chưa xác thực email hoặc chưa có tài khoản
                var registeredUser = await _userManager.FindByLoginAsync(provider, tokenInfo.Subject);
                if(registeredUser != null)
                {
                    return new Response()
                    {
                        RedirectLink = GlobalVariable.CLIENT_URL + $"account/confirm-email?email={HttpUtility.UrlEncode(registeredUser.Email)}",
                        StatusCode = System.Net.HttpStatusCode.Redirect,
                        Success = true
                    };
                }

                return await ConfirmOrRegisterForUser(provider, tokenInfo, tokenResponse); 
            }
        }

        private async Task<Response> ConfirmOrRegisterForUser(string provider, DecodeJwtToken tokenInfo, TokenReponse tokenResponse)
        {
            string externalMail = tokenInfo.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email)?.Value;

            var userWithExternalLogin = (externalMail != null) ? await _userManager.FindByEmailAsync(externalMail) : null;

            if ((userWithExternalLogin != null) && (externalMail != null))
            {
                if (!userWithExternalLogin.EmailConfirmed)
                {
                    var codeActive = await _userManager.GenerateEmailConfirmationTokenAsync(userWithExternalLogin);
                    await _userManager.ConfirmEmailAsync(userWithExternalLogin, codeActive);
                }

                var info = new UserLoginInfo(provider, tokenInfo.Subject, provider);
                var resultAdd = await _userManager.AddLoginAsync(userWithExternalLogin, info);

                if (resultAdd.Succeeded)
                {
                    var token = await _jwtRepo.GenerateUserTokenAsync(userWithExternalLogin, DateTime.UtcNow.AddMinutes(GlobalVariable.TOKEN_EXPIRED));
                    var refreshToken = await _userManager.GetAuthenticationTokenAsync(userWithExternalLogin, Provider.Google.ToString(), GlobalVariable.REFRESH_TOKEN);

                    if (string.IsNullOrEmpty(refreshToken))
                    {
                        refreshToken = _jwtRepo.GenerateRefreshToken();
                        await _userManager.SetAuthenticationTokenAsync(userWithExternalLogin, Provider.Google.ToString(), GlobalVariable.REFRESH_TOKEN, refreshToken);
                    }

                    return new Response()
                    {
                        Message = "",
                        Token = token,
                        RefreshToken = refreshToken,
                        StatusCode = System.Net.HttpStatusCode.OK,
                        Success = true
                    };
                }
                else
                {
                    return new Response()
                    {
                        Message = "Unable to perform external link to account " + externalMail,
                        StatusCode = System.Net.HttpStatusCode.InternalServerError,
                        Success = false
                    };
                }

            }

            var googleInfo = await GetMoreUserInfoAsync(tokenResponse);
            return await RegisterUserAsync(tokenInfo, googleInfo);
        }

        private async Task<Response> RegisterUserAsync(DecodeJwtToken tokenInfo, GoogleUserInfo googleInfo)
        {
            try
            {
                var firstName = tokenInfo.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.FamilyName)?.Value;
                var lastName = tokenInfo.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.GivenName)?.Value;
                var email = tokenInfo.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email)?.Value;
                var emailVerify = tokenInfo.Claims.FirstOrDefault(c => c.Type == "email_verified")?.Value;
                var sub = tokenInfo.Subject;

                var registerModel = new RegisterModel()
                {
                    FirstName = firstName,
                    LastName = lastName,
                    UserName = email,
                    Email = email,
                    Address = googleInfo?.Address,
                    DateOfBirth = googleInfo?.DayOfBirth == null ? DateTime.MinValue : googleInfo.DayOfBirth,
                    Gender = googleInfo?.Gender == null ? Gender.Male : googleInfo.Gender,
                    Password = GeneratePassword(10),
                    PhoneNumber = googleInfo?.PhoneNumber?.Replace(" ", "")
                };


                var createdResponse = await _accountRepo.RegisterAsync(registerModel, Provider.Google);
                if (!createdResponse.Success) return createdResponse;

                var newUser = await _userManager.FindByNameAsync(email);

                if (!newUser.EmailConfirmed)
                {
                    var codeActive = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
                    await _userManager.ConfirmEmailAsync(newUser, codeActive);
                }

                var info = new UserLoginInfo(Provider.Google.ToString(), tokenInfo.Subject, Provider.Google.ToString());
                var resultAdd = await _userManager.AddLoginAsync(newUser, info);

                if (resultAdd.Succeeded)
                {
                    var token = await _jwtRepo.GenerateUserTokenAsync(newUser, DateTime.UtcNow.AddMinutes(GlobalVariable.TOKEN_EXPIRED));
                    var refreshToken = await _userManager.GetAuthenticationTokenAsync(newUser, Provider.Google.ToString(), GlobalVariable.REFRESH_TOKEN);
                    refreshToken = _jwtRepo.GenerateRefreshToken();
                    await _userManager.SetAuthenticationTokenAsync(newUser, Provider.Google.ToString(), GlobalVariable.REFRESH_TOKEN, refreshToken);

                    return new Response()
                    {
                        Message = "",
                        Token = token,
                        RefreshToken = refreshToken,
                        StatusCode = System.Net.HttpStatusCode.OK,
                        Success = true
                    };
                }
                else
                {
                    return new Response()
                    {
                        Message = "Unable to perform external link to account " + newUser.UserName,
                        StatusCode = System.Net.HttpStatusCode.InternalServerError,
                        Success = false
                    };
                }
            }
            catch(Exception ex)
            {
                return new Response() 
                {
                    Message = ex.Message,
                    StatusCode = System.Net.HttpStatusCode.InternalServerError,
                    Success = false
                };
            }
        }

        private async Task<GoogleUserInfo> GetMoreUserInfoAsync(TokenReponse tokenResponse)
        {
            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Get, "https://people.googleapis.com/v1/people/me?personFields=addresses,birthdays,phoneNumbers,genders");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse.Access_Token);

                try
                {
                    var response = await client.SendAsync(request);
                    var result = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        var personInfo = JsonConvert.DeserializeObject<dynamic>(result);
                        var birthDay = personInfo?.birthdays[0]?.date;
                        var addresses = personInfo?.addresses[0]?.formattedValue;
                        var phoneNumber = personInfo?.phoneNumbers[0]?.value;
                        var gender = personInfo?.genders[0]?.value;
                        var info = new GoogleUserInfo();

                        if (birthDay != null)
                        {
                            info.DayOfBirth = new DateTime(Convert.ToInt32(birthDay.year), Convert.ToInt32(birthDay.month),Convert.ToInt32(birthDay.day));
                        }

                        if (addresses != null)
                        {
                            info.Address = addresses;
                        }

                        if (phoneNumber != null)
                        {
                            info.PhoneNumber = phoneNumber;
                        }

                        if (gender != null)
                        {
                            string genderName = gender.ToString();
                            info.Gender = genderName?.ToLower() == "male" ? Gender.Male: Gender.Female;
                        }

                        return info;
                    }

                    return null;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    return null;
                }
            }
        }

        private string GeneratePassword(int length)
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber).Substring(0,length);
        }
    }

}
