using System.Web;
using EnglishCenter.Business.IServices;
using EnglishCenter.Presentation.Helpers;
using EnglishCenter.Presentation.Models;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> RenewAccessTokenAsync([FromBody] TokenRequest token)
        {
            if (token == null) return NotFound();

              token.AccessToken = HttpUtility.UrlDecode(token.AccessToken);
            token.RefreshToken = HttpUtility.UrlDecode(token.RefreshToken);

            var renewResult = await _jwtService.RenewTokenAsync(token.AccessToken, token.RefreshToken);

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
