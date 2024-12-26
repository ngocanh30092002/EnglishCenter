using System.Security.Claims;
using EnglishCenter.Business.IServices;
using EnglishCenter.Presentation.Global;
using EnglishCenter.Presentation.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnglishCenter.Presentation.Controllers.HomePage
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class IssueReportsController : ControllerBase
    {
        private readonly IIssueReportService _issueReportService;
        private readonly IIssueResponseService _issueResponseService;

        public IssueReportsController(IIssueReportService issueReportService, IIssueResponseService issueResponseService)
        {
            _issueReportService = issueReportService;
            _issueResponseService = issueResponseService;
        }

        [HttpGet()]
        public async Task<IActionResult> GetAllAsync()
        {
            var response = await _issueReportService.GetAllAsync();
            return await response.ChangeActionAsync();
        }

        [HttpGet("admin")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> GetAllByAdminAsync()
        {
            var response = await _issueReportService.GetAllByAdminAsync();
            return await response.ChangeActionAsync();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] long id)
        {
            var response = await _issueReportService.GetById(id);
            return await response.ChangeActionAsync();
        }

        [HttpGet("user")]
        public async Task<IActionResult> GetByUserAsync()
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";
            if (userId == "")
            {
                return BadRequest();
            }

            var response = await _issueReportService.GetByUserAsync(userId);
            return await response.ChangeActionAsync();
        }

        [HttpGet("type")]
        public async Task<IActionResult> GetTypeAsync()
        {
            var response = await _issueReportService.GetTypeAsync();
            return await response.ChangeActionAsync();
        }

        [HttpGet("status")]
        public async Task<IActionResult> GetStatusAsync()
        {
            var response = await _issueReportService.GetStatusAsync();
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> ChangeStatusAsync([FromRoute] long id, int status)
        {
            var response = await _issueReportService.ChangeStatusAsync(id, status);
            return await response.ChangeActionAsync();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromForm] IssueReportDto model)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";
            if (userId == "")
            {
                return BadRequest();
            }

            model.UserId = userId;

            var response = await _issueReportService.CreateAsync(model);
            return await response.ChangeActionAsync();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] long id, [FromForm] IssueReportDto model)
        {
            var response = await _issueReportService.UpdateAsync(id, model);
            return await response.ChangeActionAsync();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] long id)
        {
            var response = await _issueReportService.DeleteAsync(id);
            return await response.ChangeActionAsync();
        }

        [HttpGet("response")]
        public async Task<IActionResult> GetAllResponseAsync()
        {
            var response = await _issueResponseService.GetAllAsync();
            return await response.ChangeActionAsync();
        }

        [HttpGet("response/{id}")]
        public async Task<IActionResult> GetByIdResponseAsync([FromRoute] long id)
        {
            var response = await _issueResponseService.GetById(id);
            return await response.ChangeActionAsync();
        }

        [HttpPost("response")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> CreateResponseAsync([FromForm] IssueResponseDto model)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";
            if (userId == "")
            {
                return BadRequest();
            }

            model.UserId = userId;

            var response = await _issueResponseService.CreateAsync(model);
            return await response.ChangeActionAsync();
        }

        [HttpPut("response/{id}")]
        public async Task<IActionResult> UpdateResponseAsync([FromRoute] long id, [FromForm] IssueResponseDto model)
        {
            var response = await _issueResponseService.UpdateAsync(id, model);
            return await response.ChangeActionAsync();
        }

        [HttpDelete("response/{id}")]
        public async Task<IActionResult> DeleteResponseAsync([FromRoute] long id)
        {
            var response = await _issueResponseService.DeleteAsync(id);
            return await response.ChangeActionAsync();
        }
    }
}
