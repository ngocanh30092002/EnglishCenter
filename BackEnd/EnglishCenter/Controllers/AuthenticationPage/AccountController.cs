using EnglishCenter.Global.Enum;
using EnglishCenter.Helpers;
using EnglishCenter.Models;
using EnglishCenter.Repositories.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace EnglishCenter.Controllers.Account
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _accountRepo;

        public AccountController(IAccountRepository accountRepo)
        {
            _accountRepo = accountRepo;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync([FromBody]LoginModel model)
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
                    Message = string.Join("<br>", errorMessage),
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

                return await response.ChangeActionAsync();
            }

            var loginResponse =  await _accountRepo.LoginAsync(model);

            if(loginResponse.Success)
            {
                CookieOptions options = new CookieOptions()
                {
                    HttpOnly = false,
                    Secure = true,
                    Expires = DateTime.UtcNow.AddHours(1),
                    Path = "/"
                };

                CookieHelper.AddCookie(HttpContext, "access-token", loginResponse.Token, options);
                CookieHelper.AddCookie(HttpContext, "refresh-token", loginResponse.RefreshToken, options);
            }

            return await loginResponse.ChangeActionAsync();
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync([FromForm] RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                var errorMessage = ModelState.Values
                                             .SelectMany(v => v.Errors)
                                             .Select(err => err.ErrorMessage)
                                             .ToList();

                var response = new Response() { Success = false, Message = string.Join("<br>", errorMessage), StatusCode = System.Net.HttpStatusCode.BadRequest };
                return await response.ChangeActionAsync();
            }

            var registerResponse = await _accountRepo.RegisterAsync(model);

            return await registerResponse.ChangeActionAsync();
        }
    }
}
