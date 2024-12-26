using System.ComponentModel.DataAnnotations;
using EnglishCenter.Business.IServices;
using EnglishCenter.Presentation.Global;
using EnglishCenter.Presentation.Helpers;
using EnglishCenter.Presentation.Models;
using Microsoft.AspNetCore.Authorization;
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

            if (loginResponse.Success)
            {
                CookieOptions options = new CookieOptions()
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Path = "/"
                };

                CookieHelper.AddCookie(HttpContext, "access-token", loginResponse.Token, options);
                CookieHelper.AddCookie(HttpContext, "refresh-token", loginResponse.RefreshToken, options);
            }

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

        [HttpPost("admin/register")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> RegisterWithRolesAsync([FromForm] RegisterModel model)
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

            if (model.Image != null)
            {
                var isImageFile = await UploadHelper.IsImageAsync(model.Image);
                if (!isImageFile)
                {
                    return BadRequest(new { message = "The image file is invalid. Only JPEG, PNG, GIF, and SVG are allowed.", success = false });
                }
            }

            if (model.BackgroundImage != null)
            {
                var isImageFile = await UploadHelper.IsImageAsync(model.BackgroundImage);
                if (!isImageFile)
                {
                    return BadRequest(new { message = "The background image file is invalid. Only JPEG, PNG, GIF, and SVG are allowed.", success = false });
                }
            }

            var registerResponse = await _accountService.RegisterWithRoleAsync(model);

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

        [HttpPost("logout")]
        public async Task<IActionResult> LogoutAsync()
        {
            var cookieOptions = new CookieOptions()
            {
                HttpOnly = true,
                Secure = true,
                Expires = DateTime.UtcNow.AddHours(-1)
            };

            if (Request.Cookies["access-token"] != null)
            {
                Response.Cookies.Append("access-token", "", cookieOptions);
            }

            if (Request.Cookies["refresh-token"] != null)
            {
                Response.Cookies.Append("refresh-token", "", cookieOptions);
            }

            var response = new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Success = true,
                Message = ""
            };

            return await response.ChangeActionAsync();
        }
    }
}
