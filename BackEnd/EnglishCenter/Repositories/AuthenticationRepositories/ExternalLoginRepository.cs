using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Policy;
using System.Text.Json;
using System.Web;
using EnglishCenter.Global;
using EnglishCenter.Models;
using EnglishCenter.Repositories.IRepositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EnglishCenter.Repositories.AuthenticationRepositories
{
    public class ExternalLoginRepository : IExternalLoginRepository
    {
        private readonly IJsonWebTokenRepository _jwtRepo;
        private readonly SignInManager<User> _signInManage;
        private readonly UserManager<User> _userManager;

        public ExternalLoginRepository(IJsonWebTokenRepository jwtRepo, SignInManager<User> signInManager, UserManager<User> userManager) 
        {
            _jwtRepo = jwtRepo;
            _signInManage = signInManager;
            _userManager = userManager;
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
                        var tokenResponse = JsonSerializer.Deserialize<TokenReponse>(responseContent);
                        return await SignInAndRegisterForUser(tokenResponse.Token_Id, "Google");
                    }
                }

                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.InternalServerError,
                    Success = false,
                    Message = "Login with google account failed",
                    RedirectLink = GlobalVariable.CLIENT_URL
                };
            }
            catch (Exception ex)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.InternalServerError,
                    Success = false,
                    Message = ex.Message,
                    RedirectLink = GlobalVariable.CLIENT_URL
                };
            }
        }

        private async Task<Response> SignInAndRegisterForUser(string jwtToken, string provider)
        {
            var tokenInfo = await _jwtRepo.DecodeToken(jwtToken);

            var result = await _signInManage.ExternalLoginSignInAsync(provider, tokenInfo.Subject, isPersistent: false, bypassTwoFactor: false);

            if(result.Succeeded)
            {
                var userInfo = await _userManager.FindByLoginAsync(provider, tokenInfo.Subject);
                var token = await _jwtRepo.GenerateUserTokenAsync(userInfo, DateTime.UtcNow.AddMinutes(10));

                return new Response()
                {
                    RedirectLink = GlobalVariable.CLIENT_URL + "dashboard",
                    Message = "",
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Success = true,
                    Token = token
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


                return await RegisterUserWithExternal(provider, tokenInfo);
            }
        }

        private async Task<Response> RegisterUserWithExternal(string provider, DecodeJwtToken tokenInfo)
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
                    var token = await _jwtRepo.GenerateUserTokenAsync(userWithExternalLogin, DateTime.UtcNow.AddMinutes(10));
                    return new Response()
                    {
                        Message = "",
                        Token = token,
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

            return new Response()
            {
                RedirectLink = GlobalVariable.CLIENT_URL + "sign-up",
                StatusCode = System.Net.HttpStatusCode.Redirect,
                Success = true,
                Message = "",
                Token = ""
            };
  
        }
    }

}
