using System.ComponentModel.DataAnnotations;
using EnglishCenter.Business.IServices;
using EnglishCenter.Presentation.Helpers;
using EnglishCenter.Presentation.Models;
using Microsoft.AspNetCore.Mvc;

namespace EnglishCenter.Presentation.Controllers.AuthenticationPage
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                var errorMessage = ModelState.Values
                                             .SelectMany(v => v.Errors)
                                             .Select(err => err.ErrorMessage)
                                             .ToList();

                var response = new Response()
                {
                    Success = false,
                    Message = errorMessage,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

                return await response.ChangeActionAsync();
            }

            var loginResponse = await _accountService.LoginAsync(model);

            //if (loginResponse.Success)
            //{
            //    CookieOptions options = new CookieOptions()
            //    {
            //        HttpOnly = false,
            //        Secure = true,
            //        Expires = DateTime.UtcNow.AddHours(1),
            //        Path = "/"
            //    };

            //    CookieHelper.AddCookie(HttpContext, "access-token", loginResponse.Token, options);
            //    CookieHelper.AddCookie(HttpContext, "refresh-token", loginResponse.RefreshToken, options);
            //}

            return await loginResponse.ChangeActionAsync();
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromForm] RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                var errorMessage = ModelState.Values
                                             .SelectMany(v => v.Errors)
                                             .Select(err => err.ErrorMessage)
                                             .ToList();

                var response = new Response() { Success = false, Message = errorMessage, StatusCode = System.Net.HttpStatusCode.BadRequest };
                return await response.ChangeActionAsync();
            }

            var registerResponse = await _accountService.RegisterAsync(model);

            return await registerResponse.ChangeActionAsync();
        }

        [HttpPost("renew-password")]
        public async Task<IActionResult> RenewPasswordAsync([FromBody] string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest(new { Message = "Email is required" });
            }

            var emailAttribute = new EmailAddressAttribute();
            if (!emailAttribute.IsValid(email))
            {
                return BadRequest(new { Message = "Invalid email format" });
            }

            var response = await _accountService.RenewPasswordAsync(email);

            return await response.ChangeActionAsync();
        }
    }
}
