using System.Security.Claims;
using EnglishCenter.Business.IServices;
using EnglishCenter.Presentation.Global;
using EnglishCenter.Presentation.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnglishCenter.Presentation.Controllers.ChatPage
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ChatsController : ControllerBase
    {
        private readonly IChatMessageService _chatService;

        public ChatsController(IChatMessageService chatService)
        {
            _chatService = chatService;
        }

        [HttpGet("admin")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> GetAllAsync()
        {
            var response = await _chatService.GetAllAsync();
            return await response.ChangeActionAsync();
        }

        [HttpGet("{id}")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> GetAsync(long id)
        {
            var response = await _chatService.GetAsync(id);
            return await response.ChangeActionAsync();
        }

        [HttpGet()]
        public async Task<IActionResult> GetChatAsync([FromQuery] string receiverId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

            var response = await _chatService.GetMessagesAsync(userId, receiverId);
            return await response.ChangeActionAsync();
        }

        [HttpGet("class/{classId}/student")]
        public async Task<IActionResult> GetMembersInClassAsync([FromRoute] string classId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";
            var response = await _chatService.GetMembersInClassAsync(userId, classId);
            return await response.ChangeActionAsync();
        }

        [HttpGet("class/{classId}/teacher")]
        public async Task<IActionResult> GetTeacherInClassAsync([FromRoute] string classId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";
            var response = await _chatService.GetTeacherInClassAsync(userId, classId);
            return await response.ChangeActionAsync();
        }

        [HttpPost()]
        public async Task<IActionResult> CreateAsync([FromForm] ChatMessageDto chatModel)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

            chatModel.SenderId = userId;

            var response = await _chatService.CreateAsync(chatModel);
            return await response.ChangeActionAsync();
        }

        [HttpPost("admin")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> CreateByAdminAsync([FromForm] ChatMessageDto chatModel)
        {
            var response = await _chatService.CreateAsync(chatModel);
            return await response.ChangeActionAsync();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> DeleteAsync([FromRoute] long chatId)
        {
            var response = await _chatService.DeleteAsync(chatId);
            return await response.ChangeActionAsync();
        }

    }
}
