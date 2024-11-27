using System.Security.Claims;
using EnglishCenter.Business.IServices;
using EnglishCenter.Presentation.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnglishCenter.Presentation.Controllers.ClassPage
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SubmissionFilesController : ControllerBase
    {
        private readonly ISubmissionFileService _fileService;

        public SubmissionFilesController(ISubmissionFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var response = await _fileService.GetAllAsync();
            return await response.ChangeActionAsync();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync([FromRoute] long id)
        {
            var response = await _fileService.GetAsync(id);
            return await response.ChangeActionAsync();
        }

        [HttpGet("tasks/{taskId}")]
        public async Task<IActionResult> GetByEnrollAndIdAsync([FromRoute] long taskId, [FromQuery] long enrollId)
        {
            var response = await _fileService.GetByEnrollAndIdAsync(enrollId, taskId);
            return await response.ChangeActionAsync();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromForm] SubmissionFileDto model)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

            var response = await _fileService.CreateAsync(userId, model);
            return await response.ChangeActionAsync();
        }

        [HttpPost("files")]
        public async Task<IActionResult> HandleUploadMoreFilesAsync([FromForm] SubmissionFileDto model)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

            var response = await _fileService.HandleUploadMoreFilesAsync(userId, model);
            return await response.ChangeActionAsync();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] long id, [FromForm] SubmissionFileDto model)
        {
            var response = await _fileService.UpdateAsync(id, model);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{id}/file")]
        public async Task<IActionResult> ChangeFilePathAsync([FromRoute] long id, IFormFile file)
        {
            var response = await _fileService.ChangeFilePathAsync(id, file);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{id}/link-url")]
        public async Task<IActionResult> ChangeTitleAsync([FromRoute] long id, [FromBody] string linkUrl)
        {
            var response = await _fileService.ChangeLinkUrlAsync(id, linkUrl);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{id}/upload-by")]
        public async Task<IActionResult> ChangeUploadByAsync([FromRoute] long id, [FromBody] string uploadBy)
        {
            var response = await _fileService.ChangeUploadByAsync(id, uploadBy);
            return await response.ChangeActionAsync();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] long id)
        {
            var response = await _fileService.DeleteAsync(id);
            return await response.ChangeActionAsync();
        }
    }
}
