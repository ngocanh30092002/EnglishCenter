using System.Text;
using System.Text.Json;
using Azure.Core;
using EnglishCenter.Global;
using EnglishCenter.Global.Enum;
using EnglishCenter.Helpers;
using EnglishCenter.Models;
using EnglishCenter.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json.Linq;
using NuGet.Protocol.Plugins;

namespace EnglishCenter.Controllers.AuthenticationPage
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    [ApiController]
    public class ExternalLoginController : ControllerBase
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _config;
        private readonly IExternalLoginRepository _repo;
        private const string _googleTokenUrl = "https://oauth2.googleapis.com/token";

        public ExternalLoginController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IConfiguration config,
            IExternalLoginRepository repo) 
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _config = config;
            _repo = repo;
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
            var response = await _repo.SignInGoogleAsync(requestParams, _googleTokenUrl);

            if (response.Success)
            {
                CookieOptions options = new CookieOptions()
                {
                    HttpOnly = false,
                    Secure = true,
                    Expires = DateTime.UtcNow.AddHours(1),
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
