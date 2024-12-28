using System.Diagnostics;
using System.Net.Http.Headers;
using System.Web;
using EnglishCenter.Business.IServices;
using EnglishCenter.DataAccess.Database;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.Presentation.Global;
using EnglishCenter.Presentation.Global.Enum;
using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.JsonWebTokens;
using Newtonsoft.Json;

namespace EnglishCenter.Business.Services.Authorization
{
    public class ExternalLoginService : IExternalLoginService
    {
        private readonly IJsonTokenService _jwtService;
        private readonly SignInManager<User> _signInManage;
        private readonly UserManager<User> _userManager;
        private readonly EnglishCenterContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IAccountService _accountService;
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        public ExternalLoginService(
            IJsonTokenService jwtService,
            SignInManager<User> signInManager,
            UserManager<User> userManager,
            EnglishCenterContext context,
            RoleManager<IdentityRole> roleManager,
            IAccountService accountService,
            IUserService userService,
            IConfiguration configuration)
        {
            _jwtService = jwtService;
            _signInManage = signInManager;
            _userManager = userManager;
            _context = context;
            _roleManager = roleManager;
            _accountService = accountService;
            _userService = userService;
            _configuration = configuration;
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
            var tokenInfo = await _jwtService.DecodeToken(tokenResponse.Token_Id);

            var result = await _signInManage.ExternalLoginSignInAsync(provider, tokenInfo.Subject, isPersistent: false, bypassTwoFactor: false);

            if (result.Succeeded)
            {
                var userInfo = await _userManager.FindByLoginAsync(provider, tokenInfo.Subject);
                var token = await _jwtService.GenerateUserTokenAsync(userInfo, DateTime.UtcNow.AddMinutes(GlobalVariable.TOKEN_EXPIRED), Provider.Google);
                var userToken = await _jwtService.GetRefreshTokenFromUser(userInfo, Provider.Google);

                if (string.IsNullOrEmpty(userToken))
                {
                    userToken = _jwtService.GenerateRefreshToken();
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
                if (registeredUser != null)
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

            var userWithExternalLogin = externalMail != null ? await _userManager.FindByEmailAsync(externalMail) : null;

            if (userWithExternalLogin != null && externalMail != null)
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
                    var token = await _jwtService.GenerateUserTokenAsync(userWithExternalLogin, DateTime.UtcNow.AddMinutes(GlobalVariable.TOKEN_EXPIRED));
                    var refreshToken = await _userManager.GetAuthenticationTokenAsync(userWithExternalLogin, Provider.Google.ToString(), GlobalVariable.REFRESH_TOKEN);

                    if (string.IsNullOrEmpty(refreshToken))
                    {
                        refreshToken = _jwtService.GenerateRefreshToken();
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
                    DateOfBirth = googleInfo?.DayOfBirth == null ? DateOnly.MinValue : DateOnly.FromDateTime(googleInfo.DayOfBirth),
                    Gender = googleInfo?.Gender == null ? Gender.Male : googleInfo.Gender,
                    Password = GlobalMethods.GeneratePassword(10),
                    PhoneNumber = googleInfo?.PhoneNumber?.Replace(" ", "")
                };


                var createdResponse = await _accountService.RegisterAsync(registerModel, Provider.Google);
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
                    var token = await _jwtService.GenerateUserTokenAsync(newUser, DateTime.UtcNow.AddMinutes(GlobalVariable.TOKEN_EXPIRED));
                    var refreshToken = await _userManager.GetAuthenticationTokenAsync(newUser, Provider.Google.ToString(), GlobalVariable.REFRESH_TOKEN);
                    refreshToken = _jwtService.GenerateRefreshToken();
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
            catch (Exception ex)
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
                            info.DayOfBirth = new DateTime(Convert.ToInt32(birthDay.year), Convert.ToInt32(birthDay.month), Convert.ToInt32(birthDay.day));
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
                            info.Gender = genderName?.ToLower() == "male" ? Gender.Male : Gender.Female;
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

        public async Task<bool> IsFacebookTokenValidAsync(string userAccessToken)
        {
            var appId = _configuration["Authentication:Facebook:AppId"];
            var appSecret = _configuration["Authentication:Facebook:AppSecret"];
            var appAccessToken = $"{appId}|{appSecret}";

            var fbDebugTokenUrl =
                $"https://graph.facebook.com/debug_token?input_token={userAccessToken}&access_token={appAccessToken}";

            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(fbDebugTokenUrl);

            if (!response.IsSuccessStatusCode)
                return false;

            var tokenInfo = await response.Content.ReadFromJsonAsync<FbTokenVerificationRes>();

            return tokenInfo?.Data?.Is_Valid ?? false;
        }

        public async Task<FbUserInfo> GetFbUserInfoAsync(string userAccessToken)
        {
            var fbUserInfoUrl = $"https://graph.facebook.com/me?fields=name,birthday,gender,email,location,picture.width(320).height(320),first_name,last_name&access_token={userAccessToken}";
            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(fbUserInfoUrl);

            if (!response.IsSuccessStatusCode)
                return null;

            var tokenString = await response.Content.ReadAsStringAsync();

            return await response.Content.ReadFromJsonAsync<FbUserInfo>();
        }

        public async Task<Response> SignInFacebookAsync(FbUserInfo userInfo)
        {
            var result = await _signInManage.ExternalLoginSignInAsync(Provider.Facebook.ToString(), userInfo.Id, isPersistent: false, bypassTwoFactor: false);
            if (result.Succeeded)
            {
                var userInfoModel = await _userManager.FindByLoginAsync(Provider.Facebook.ToString(), userInfo.Id);
                if (userInfoModel != null)
                {
                    var token = await _jwtService.GenerateUserTokenAsync(userInfoModel, DateTime.UtcNow.AddMinutes(GlobalVariable.TOKEN_EXPIRED), Provider.Google);
                    var userToken = await _jwtService.GetRefreshTokenFromUser(userInfoModel, Provider.Google);

                    if (string.IsNullOrEmpty(userToken))
                    {
                        userToken = _jwtService.GenerateRefreshToken();
                        await _userManager.SetAuthenticationTokenAsync(userInfoModel, Provider.Google.ToString(), GlobalVariable.REFRESH_TOKEN, userToken);
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
                else
                {
                    return new Response()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Message = "Can't login with facebook",
                        Success = false
                    };
                }
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
                var registeredUser = await _userManager.FindByLoginAsync(Provider.Facebook.ToString(), userInfo.Id);
                if (registeredUser != null)
                {
                    return new Response()
                    {
                        RedirectLink = GlobalVariable.CLIENT_URL + $"account/confirm-email?email={HttpUtility.UrlEncode(registeredUser.Email)}",
                        StatusCode = System.Net.HttpStatusCode.Redirect,
                        Success = true
                    };
                }

                return await ConfirmOrRegisterForUser(userInfo);
            }
        }

        public async Task<Response> ConfirmOrRegisterForUser(FbUserInfo userInfo)
        {
            string externalMail = userInfo.Email;
            var userWithExternalLogin = externalMail != null ? await _userManager.FindByEmailAsync(externalMail) : null;

            if (userWithExternalLogin != null && externalMail != null)
            {
                if (!userWithExternalLogin.EmailConfirmed)
                {
                    var codeActive = await _userManager.GenerateEmailConfirmationTokenAsync(userWithExternalLogin);
                    await _userManager.ConfirmEmailAsync(userWithExternalLogin, codeActive);
                }

                var info = new UserLoginInfo(Provider.Facebook.ToString(), userInfo.Id, Provider.Facebook.ToString());
                var resultAdd = await _userManager.AddLoginAsync(userWithExternalLogin, info);

                if (resultAdd.Succeeded)
                {
                    var token = await _jwtService.GenerateUserTokenAsync(userWithExternalLogin, DateTime.UtcNow.AddMinutes(GlobalVariable.TOKEN_EXPIRED));
                    var refreshToken = await _userManager.GetAuthenticationTokenAsync(userWithExternalLogin, Provider.Google.ToString(), GlobalVariable.REFRESH_TOKEN);

                    if (string.IsNullOrEmpty(refreshToken))
                    {
                        refreshToken = _jwtService.GenerateRefreshToken();
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

            return await RegisterUserAsync(userInfo);
        }

        public async Task<Response> RegisterUserAsync(FbUserInfo userInfo)
        {
            if (userInfo == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "User info isn't exist",
                    Success = false
                };
            }

            try
            {
                var gender = Gender.Male;
                if (userInfo.Gender == "male")
                {
                    gender = Gender.Male;
                }
                else if (userInfo.Gender == "female")
                {
                    gender = Gender.Female;
                }
                else
                {
                    gender = Gender.Other;
                }

                var registerModel = new RegisterModel()
                {
                    FirstName = userInfo.First_Name ?? "",
                    LastName = userInfo.Last_Name ?? "",
                    UserName = userInfo.Email ?? "",
                    Email = userInfo.Email ?? "",
                    Address = userInfo.Location?["name"]?.ToString(),
                    DateOfBirth = userInfo?.Birthday == null ? DateOnly.MinValue : DateOnly.Parse(userInfo.Birthday),
                    Gender = gender,
                    Password = GlobalMethods.GeneratePassword(10),
                };


                var createdResponse = await _accountService.RegisterAsync(registerModel, Provider.Facebook);
                if (!createdResponse.Success) return createdResponse;

                var newUser = await _userManager.FindByNameAsync(userInfo.Email ?? "");
                if (newUser == null)
                {
                    return new Response()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Message = "Can't find any users",
                        Success = false
                    };
                }

                var fileImage = await DownloadFileAsync(userInfo.Picture["url"].ToString() ?? "");
                if (fileImage != null)
                {
                    var changeImageRes = await _userService.ChangeUserImageAsync(newUser.Id, fileImage);
                    if (changeImageRes.Success == false) return changeImageRes;
                }


                if (!newUser.EmailConfirmed)
                {
                    var codeActive = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
                    await _userManager.ConfirmEmailAsync(newUser, codeActive);
                }

                var info = new UserLoginInfo(Provider.Facebook.ToString(), userInfo.Id, Provider.Facebook.ToString());
                var resultAdd = await _userManager.AddLoginAsync(newUser, info);

                if (resultAdd.Succeeded)
                {
                    var token = await _jwtService.GenerateUserTokenAsync(newUser, DateTime.UtcNow.AddMinutes(GlobalVariable.TOKEN_EXPIRED));
                    var refreshToken = await _userManager.GetAuthenticationTokenAsync(newUser, Provider.Google.ToString(), GlobalVariable.REFRESH_TOKEN);
                    refreshToken = _jwtService.GenerateRefreshToken();
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
            catch (Exception ex)
            {
                return new Response()
                {
                    Message = ex.Message,
                    StatusCode = System.Net.HttpStatusCode.InternalServerError,
                    Success = false
                };
            }
        }

        private async Task<IFormFile> DownloadFileAsync(string url)
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var stream = await response.Content.ReadAsStreamAsync();

                var fileName = Path.GetFileName(url);

                var formFile = new FormFile(stream, 0, stream.Length, "file", fileName)
                {
                    Headers = new HeaderDictionary(),
                    ContentType = response.Content.Headers.ContentType?.ToString() ?? "application/octet-stream"
                };

                return formFile;
            }
        }
    }
}
