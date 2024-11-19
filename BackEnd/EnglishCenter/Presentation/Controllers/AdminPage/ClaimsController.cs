using EnglishCenter.Business.IServices;
using EnglishCenter.Business.Services;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.IRepositories;
using EnglishCenter.Presentation.Global;
using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EnglishCenter.Presentation.Controllers.AdminPage
{
    [Authorize(Roles = AppRole.ADMIN)]
    [Route("api/[controller]")]
    [ApiController]
    public class ClaimsController : ControllerBase
    {
        private readonly IClaimService _claimService;
        private readonly UserManager<User> _userManager;

        public ClaimsController(IClaimService claimService, UserManager<User> userManager)
        {
            _claimService = claimService;
            _userManager = userManager;
        }

        [HttpGet("users/{userId}")]
        public async Task<IActionResult> GetClaimsUserAsync([FromRoute] string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return BadRequest("Can't find any users");
            }

            var claims = await _claimService.GetClaimsUserAsync(user);
            var response = new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Success = true,
                Message = claims
            };

            return await response.ChangeActionAsync();
        }

        [HttpGet("roles/{roleName}")]
        public async Task<IActionResult> GetRoleClaimsAsync([FromRoute] string roleName)
        {
            var claims = await _claimService.GetRoleClaimsAsync(roleName);

            var response = new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Success = true,
                Message = claims
            };

            return await response.ChangeActionAsync();
        }

        [HttpGet("users/{userId}/user-claims")]
        public async Task<IActionResult> GetUserClaimsAsync([FromRoute] string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return BadRequest("Can't find any users");
            }

            var claims = await _claimService.GetUserClaimsAsync(user);
            var response = new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Success = true,
                Message = claims
            };

            return await response.ChangeActionAsync();
        }

        [HttpPost("roles/{roleName}")]
        public async Task<IActionResult> AddClaimInRoleAsync([FromRoute] string roleName, [FromForm] ClaimDto model)
        {
            var response = await _claimService.AddClaimInRoleAsync(roleName, model);

            if (!response)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpPost("users/{userId}")]
        public async Task<IActionResult> AddClaimInUserAsync([FromRoute] string userId, [FromForm] ClaimDto model)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return BadRequest("Can't find any users");
            }

            var response = await _claimService.AddClaimToUserAsync(user, model);

            if (!response)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpDelete("roles/{roleName}")]
        public async Task<IActionResult> DeleteClaimInRoleAsync([FromRoute] string roleName, [FromForm] ClaimDto model)
        {
            var response = await _claimService.DeleteClaimInRoleAsync(roleName, model);
            if (!response)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpDelete("users/{userId}")]
        public async Task<IActionResult> DeleteClaimInUserAsync([FromRoute] string userId, [FromForm] ClaimDto model)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return BadRequest("Can't find any users");
            }

            var response = await _claimService.DeleteClaimInUserAsync(user, model);
            if (!response)
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}
