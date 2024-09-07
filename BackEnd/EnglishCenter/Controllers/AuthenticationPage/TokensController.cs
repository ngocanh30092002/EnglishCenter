using System.Web;
using EnglishCenter.Helpers;
using EnglishCenter.Models;
using EnglishCenter.Repositories.IRepositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EnglishCenter.Controllers.AuthenticationPage
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokensController : ControllerBase
    {
        private readonly IJsonWebTokenRepository _jwtRepo;
        private readonly UserManager<User> _userManager;

        public TokensController(
            IJsonWebTokenRepository jwtRepo,
            UserManager<User> userManager) 
        {
            _jwtRepo = jwtRepo;
            _userManager = userManager;
        }

        [HttpPost("renew")]
        public async Task<IActionResult> RenewAccessTokenAsync([FromBody] TokenRequest token)
        {
            if (token == null) return NotFound();

            token.AccessToken = HttpUtility.UrlDecode(token.AccessToken);
            token.RefreshToken = HttpUtility.UrlDecode(token.RefreshToken);

            var renewResult = await _jwtRepo.RenewTokenAsync(token.AccessToken, token.RefreshToken);

            if (renewResult.Success)
            {
                CookieOptions options = new CookieOptions()
                {
                    HttpOnly = false,
                    Secure = true,
                    Expires = DateTime.UtcNow.AddHours(1),
                    Path = "/"
                };

                CookieHelper.AddCookie(HttpContext, "access-token", renewResult.Token, options);
                CookieHelper.AddCookie(HttpContext, "refresh-token", renewResult.RefreshToken, options);

                return Ok();
            }

            return await renewResult.ChangeActionAsync();
        }

        [HttpPost("verify")]
        public async Task<IActionResult> VerifyAccessTokenAsync([FromBody] string accessToken)
        {
            var isValid = _jwtRepo.VerifyAccessToken(accessToken);

            return Ok(new {
                IsValid = isValid,
            });
        }
    }
}
