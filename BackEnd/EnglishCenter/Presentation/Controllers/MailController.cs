using EnglishCenter.Presentation.Helpers;
using EnglishCenter.Presentation.Models;
using Microsoft.AspNetCore.Mvc;

namespace EnglishCenter.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailController : ControllerBase
    {
        private readonly MailHelper _mailHelper;
        public MailController(MailHelper mailHelper)
        {
            _mailHelper = mailHelper;
        }

        [HttpPost("send-html")]
        public async Task<IActionResult> SendHtmlMail(MailContent mailContent)
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

            var result = await _mailHelper.SendHtmlMailAsync(mailContent);

            return result ? Ok() : BadRequest();
        }

        [HttpPost("send-text")]
        public async Task<IActionResult> SendMail(MailContent mailContent)
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

            var result = await _mailHelper.SendMailAsync(mailContent);

            return result ? Ok() : BadRequest();
        }
    }
}
