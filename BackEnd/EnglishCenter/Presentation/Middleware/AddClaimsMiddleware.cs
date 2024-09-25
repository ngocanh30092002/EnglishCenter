using System.Security.Claims;
using EnglishCenter.Business.IServices;
using EnglishCenter.Business.Services.Authorization;
using EnglishCenter.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;

namespace EnglishCenter.Presentation.Middleware
{
    public class AddClaimsMiddleware
    {
        private readonly RequestDelegate _next;

        public AddClaimsMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, UserManager<User> userManager, IClaimService claimService)
        {
            if (context.User.Identity.IsAuthenticated)
            {
                var userId = context.User.FindFirst(ClaimTypes.NameIdentifier).Value ?? "";

                if(!string.IsNullOrEmpty(userId))
                {
                    var user = await userManager.FindByIdAsync(userId);
                    if(user != null)
                    {
                        var claims = await claimService.GetClaimsUserAsync(user);
                    
                        var appIdentity = new ClaimsIdentity(claims);

                        context.User.AddIdentity(appIdentity);
                    }
                }
            }

            await _next(context);
        }
    }
}
