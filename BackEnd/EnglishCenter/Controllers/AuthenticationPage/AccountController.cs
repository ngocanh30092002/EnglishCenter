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
        private IAccountRepository _accountRepo;

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

                return new Response()
                {
                    Success = false,
                    Message = string.Join("<br>", errorMessage),
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };
            }

            return await _accountRepo.LoginAsync(model);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                var errorMessage = ModelState.Values
                                             .SelectMany(v => v.Errors)
                                             .Select(err => err.ErrorMessage)
                                             .ToList();

                return new Response()
                {
                    Success = false,
                    Message = string.Join("<br>", errorMessage),
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };
            }

            return await _accountRepo.RegisterAsync(model);
        }
    }
}
