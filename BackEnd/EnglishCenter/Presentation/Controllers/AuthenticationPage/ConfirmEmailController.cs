using System.Net;
using System.Web;
using EnglishCenter.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EnglishCenter.Presentation.Controllers.AuthenticationPage
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfirmEmailController : ControllerBase
    {
        private readonly UserManager<User> _userManager;

        public ConfirmEmailController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }


        [HttpGet("confirm")]
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
