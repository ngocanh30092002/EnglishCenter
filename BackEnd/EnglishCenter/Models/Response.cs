using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace EnglishCenter.Models
{
    public class Response : IActionResult
    {
        public bool Success { set; get; } = false;
        public string Token { set; get; } = string.Empty;
        public string Message { set; get; } = string.Empty;
        public string RedirectLink { set; get; }  = string.Empty;
        public HttpStatusCode StatusCode { set; get; }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            var objectResult = new ObjectResult(this)
            {
                StatusCode = (int)this.StatusCode
            };

            await objectResult.ExecuteResultAsync(context);
        }
    }
}
