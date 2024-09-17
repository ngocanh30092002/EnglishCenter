using EnglishCenter.Models;
using EnglishCenter.Models.DTO;
using EnglishCenter.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EnglishCenter.Controllers.AdminPage
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class ClaimsController : ControllerBase
    {
        private readonly IClaimRepository _claimRepo;
        private readonly UserManager<User> _userManager;

        public ClaimsController(IClaimRepository claimRepo, UserManager<User> userManager) 
        {
            _claimRepo = claimRepo;
            _userManager = userManager;
        }

        [HttpGet("users/{userId}")]
        public async Task<IActionResult> GetClaimsUserAsync([FromRoute] string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if(user == null)
            {
                return BadRequest("Can't find any users");
            }

            var claims = await _claimRepo.GetClaimsUserAsync(user);
            var response = new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Success = true,
                Message = claims
            };

            return await response.ChangeActionAsync() ;
        }

        [HttpGet("roles/{roleName}")]
        public async Task<IActionResult> GetRoleClaimsAsync([FromRoute] string roleName)
        {
            var claims = await _claimRepo.GetRoleClaimsAsync(roleName);

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

            var claims = await _claimRepo.GetUserClaimsAsync(user);
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
            var response = await _claimRepo.AddClaimInRoleAsync(roleName, model);

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

            var response = await _claimRepo.AddClaimToUserAsync(user, model);

            if (!response)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpDelete("roles/{roleName}")]
        public async Task<IActionResult> DeleteClaimInRoleAsync([FromRoute] string roleName,[FromForm] ClaimDto model)
        {
            var response = await _claimRepo.DeleteClaimInRoleAsync(roleName, model);
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

            var response = await _claimRepo.DeleteClaimInUserAsync(user, model);
            if (!response)
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}
