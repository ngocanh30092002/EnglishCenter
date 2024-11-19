using System.Security.Claims;
using EnglishCenter.Business.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnglishCenter.Presentation.Controllers.ChatPage
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ChatFilesController : ControllerBase
    {
        private readonly IChatFileService _chatService;

        public ChatFilesController(IChatFileService chatService)
        {
            _chatService = chatService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(IFormFile file)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";
            var response = await _chatService.CreateAsync(userId, file);

            return await response.ChangeActionAsync();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] long id)
        {
            var response = await _chatService.DeleteAsync(id);
            return await response.ChangeActionAsync();
        }
    }
}
