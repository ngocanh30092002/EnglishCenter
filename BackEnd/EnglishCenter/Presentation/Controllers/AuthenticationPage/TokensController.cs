using System.Web;
using EnglishCenter.Business.IServices;
using EnglishCenter.Presentation.Helpers;
using EnglishCenter.Presentation.Models;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;

namespace EnglishCenter.Presentation.Controllers.AuthenticationPage
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokensController : ControllerBase
    {
        private readonly IJsonTokenService _jwtService;

        public TokensController(IJsonTokenService jwtService)
        {
            _jwtService = jwtService;
        }

        [HttpPost("renew")]
        public async Task<IActionResult> RenewAccessTokenAsync()
        {
            var token = new TokenRequest();

            if (Request.Cookies["refresh-token"] == null || Request.Cookies["access-token"] == null)
            {
                return BadRequest();
            }

            token.AccessToken = Request.Cookies["access-token"]!;
            token.RefreshToken = Request.Cookies["refresh-token"]!;

            var renewResult = await _jwtService.RenewTokenAsync(token.AccessToken, token.RefreshToken);
            if (renewResult.Success)
            {
                CookieOptions options = new CookieOptions()
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Path = "/"
                };

                CookieHelper.AddCookie(HttpContext, "access-token", renewResult.Token, options);
                CookieHelper.AddCookie(HttpContext, "refresh-token", renewResult.RefreshToken, options);
            }

            return await renewResult.ChangeActionAsync();
        }

        [HttpPost("verify")]
        public IActionResult VerifyAccessTokenAsync([FromBody] string accessToken)
        {
            var isValid = _jwtService.VerifyAccessToken(accessToken);

            return Ok(new
            {
                IsValid = isValid,
            });
        }
    }
}
