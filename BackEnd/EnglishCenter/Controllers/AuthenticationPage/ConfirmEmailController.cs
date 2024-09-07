using Azure;
using System.Net;
using EnglishCenter.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Web;
using EnglishCenter.Repositories.IRepositories;

namespace EnglishCenter.Controllers.AuthenticationPage
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfirmEmailController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IJsonWebTokenRepository _jwtRepo;

        public ConfirmEmailController(UserManager<User> userManager, IJsonWebTokenRepository jwtRepo)
        {
            _userManager = userManager;
            _jwtRepo = jwtRepo;
        }


        [HttpGet("confirm-email")]
        public async Task<IActionResult> ExecuteConfirmEmailAsync(string userId, string code, string returnUrl)
        {
            var currentUser = await _userManager.FindByIdAsync(userId);
            if (currentUser == null)
            {
                return NotFound();
            }

            var result = await _userManager.ConfirmEmailAsync(currentUser, code);

            if (!result.Succeeded)
            {
                var response = new Models.Response()
                {
                    Message = result.Errors.Select(e => e.Description).ToList(),
                    StatusCode = HttpStatusCode.BadRequest
                };

                return await response.ChangeActionAsync();
            }

            return Redirect(HttpUtility.UrlDecode(returnUrl));
        }
    }
}
