using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace EnglishCenter.Presentation.Models
{
    public class Response : Controller
    {
        public bool Success { set; get; } = false;
        public string Token { set; get; } = string.Empty;
        public string RefreshToken { set; get; } = string.Empty;
        public object? Message { set; get; } = string.Empty;
        public string RedirectLink { set; get; } = string.Empty;
        public Dictionary<string, string> UrlQueryParams { set; get; }

        public HttpStatusCode StatusCode { set; get; }

        public async Task<IActionResult> ChangeActionAsync()
        {
            switch (StatusCode)
            {
                case HttpStatusCode.OK:
                    return Ok(new
                    {
                        Message,
                        Success,
                        Token,
                        RefreshToken,
                    });

                case HttpStatusCode.Redirect:
                    var urlRedirect = UrlQueryParams == null ? RedirectLink : QueryHelpers.AddQueryString(RedirectLink, UrlQueryParams);
                    return Redirect(urlRedirect);

                case HttpStatusCode.BadRequest:
                    return BadRequest(new
                    {
                        Message = Message ?? "Invalid request",
                        Success
                    });

                case HttpStatusCode.Unauthorized:
                    return Unauthorized(new
                    {
                        Message = Message ?? "Unauthorized access",
                        Success
                    });

                case HttpStatusCode.NotFound:
                    return NotFound(new
                    {
                        Message = Message ?? "Not found",
                        Success
                    });

                case HttpStatusCode.InternalServerError:
                    return base.StatusCode((int)HttpStatusCode.InternalServerError, new
                    {
                        Success,
                        Message = Message ?? "An error occurred on the server"
                    });

                default:
                    return base.StatusCode((int)HttpStatusCode.InternalServerError, new
                    {
                        Success,
                        Message = Message ?? "An unexpected error occurred"
                    });
            }
        }
    }
}
