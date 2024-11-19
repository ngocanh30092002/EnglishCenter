using EnglishCenter.Business.IServices;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.Presentation.Global;
using EnglishCenter.Presentation.Helpers;
using EnglishCenter.Presentation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EnglishCenter.Presentation.Controllers.AuthenticationPage
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    [ApiController]
    public class ExternalLoginController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IExternalLoginService _service;
        private const string _googleTokenUrl = "https://oauth2.googleapis.com/token";

        public ExternalLoginController(IConfiguration config, IExternalLoginService service)
        {
            _config = config;
            _service = service;
        }

        [HttpGet("/sign-in-google")]
        public async Task<IActionResult> SignInGoogleAsync([FromQuery] AuthGoogle authGoogle)
        {
            var requestParams = new Dictionary<string, string>()
            {
                { "client_id", _config["Authentication:Google:ClientId"]!},
                { "client_secret" , _config["Authentication:Google:ClientSecret"]!},
                { "code", authGoogle.Code },
                { "grant_type" , "authorization_code"},
                { "redirect_uri", $"{Request.Scheme}://{Request.Host}{Request.Path}" },
            };
            var response = await _service.SignInGoogleAsync(requestParams, _googleTokenUrl);

            if (response.Success)
            {
                CookieOptions options = new CookieOptions()
                {
                    HttpOnly = false,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Path = "/"
                };

                CookieHelper.AddCookie(HttpContext, "access-token", response.Token, options);
                CookieHelper.AddCookie(HttpContext, "refresh-token", response.RefreshToken, options);
                return Redirect(GlobalVariable.CLIENT_URL);
            }

            return await response.ChangeActionAsync();
        }

    }
}
