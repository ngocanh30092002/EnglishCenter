using System.Security.Claims;
using EnglishCenter.Business.IServices;
using EnglishCenter.Presentation.Global;
using EnglishCenter.Presentation.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnglishCenter.Presentation.Controllers.ClassPage
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ClassMaterialsController : ControllerBase
    {
        private readonly IClassMaterialService _materialService;

        public ClassMaterialsController(IClassMaterialService materialService)
        {
            _materialService = materialService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var response = await _materialService.GetAllAsync();
            return await response.ChangeActionAsync();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync([FromRoute] long id)
        {
            var response = await _materialService.GetAsync(id);
            return await response.ChangeActionAsync();
        }

        [HttpPost]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> CreateAsync([FromForm] ClassMaterialDto model)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

            var response = await _materialService.CreateAsync(userId, model);
            return await response.ChangeActionAsync();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> DeleteAsync([FromRoute] long id)
        {
            var response = await _materialService.DeleteAsync(id);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{id}/title")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> ChangeTitleAsync([FromRoute] long id, [FromBody] string newTitle)
        {
            var response = await _materialService.ChangeTitleAsync(id, newTitle);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{id}/file")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> ChangeFilePathAsync([FromRoute] long id, IFormFile file)
        {
            var response = await _materialService.ChangeFilePathAsync(id, file);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{id}/upload-by")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> ChangeUploadByAsync([FromRoute] long id, [FromBody] string uploadBy)
        {
            var response = await _materialService.ChangeUploadByAsync(id, uploadBy);
            return await response.ChangeActionAsync();
        }
    }
}
