using EnglishCenter.Global;
using EnglishCenter.Helpers;
using EnglishCenter.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MimeKit;

namespace EnglishCenter.Controllers
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

        [HttpPost("send-html-mail")]
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
                    Message = string.Join("<br>", errorMessage),
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

                return await response.ChangeActionAsync();
            }

            var result = await _mailHelper.SendHtmlMailAsync(mailContent);

            return result ? Ok() : BadRequest();
        }

        [HttpPost("send-mail")]
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
                    Message = string.Join("<br>", errorMessage),
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

                return await response.ChangeActionAsync();
            }

            var result = await _mailHelper.SendMailAsync(mailContent);

            return result ? Ok() : BadRequest();
        }
    }
}
