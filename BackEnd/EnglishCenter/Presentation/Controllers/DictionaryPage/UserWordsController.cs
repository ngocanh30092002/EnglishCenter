using System.Security.Claims;
using EnglishCenter.Business.IServices;
using EnglishCenter.Presentation.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnglishCenter.Presentation.Controllers.DictionaryPage
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserWordsController : ControllerBase
    {
        private readonly IUserWordService _service;

        public UserWordsController(IUserWordService service)
        {
            _service = service;
        }


        [HttpGet()]
        public async Task<IActionResult> GetAllAsync()
        {
            var response = await _service.GetAllAsync();
            return await response.ChangeActionAsync();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(long id)
        {
            var response = await _service.GetAsync(id);
            return await response.ChangeActionAsync();
        }

        [HttpGet("user/favorite")]
        public async Task<IActionResult> GetByUserWithFavoriteAsync()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

            var response = await _service.GetByUserWithFavoriteAsync(userId);
            return await response.ChangeActionAsync();
        }

        [HttpGet("user")]
        public async Task<IActionResult> GetByUserAsync()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

            var response = await _service.GetByUserAsync(userId);
            return await response.ChangeActionAsync();
        }

        [HttpGet("word-types")]
        public async Task<IActionResult> GetWordTypeAsync()
        {
            var response = await _service.GetWordTypeAsync();
            return await response.ChangeActionAsync();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromForm] UserWordDto model)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";
            model.UserId = userId;

            var response = await _service.CreateAsync(model);
            return await response.ChangeActionAsync();
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] long id, [FromForm] UserWordDto model)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";
            model.UserId = userId;

            var response = await _service.UpdateAsync(id, model);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{id}/word")]
        public async Task<IActionResult> ChangeWordAsync([FromRoute] long id, [FromBody] string newWord)
        {
            var response = await _service.ChangeWordAsync(id, newWord);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{id}/image")]
        public async Task<IActionResult> ChangeImageAsync([FromRoute] long id, IFormFile file)
        {
            var response = await _service.ChangeImageAsync(id, file);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{id}/is-favorite")]
        public async Task<IActionResult> ChangeIsFavoriteAsync([FromRoute] long id, [FromQuery] bool isFavorite)
        {
            var response = await _service.ChangeIsFavoriteAsync(id, isFavorite);
            return await response.ChangeActionAsync();
        }

        [HttpPut("favorite")]
        public async Task<IActionResult> ChangeAllFavoriteAsync([FromQuery] bool isFavorite)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

            var response = await _service.ChangeAllFavoriteAsync(userId, isFavorite);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{id}/tag")]
        public async Task<IActionResult> ChangeTagAsync([FromRoute] long id, [FromBody] string tag)
        {
            var response = await _service.ChangeTagAsync(id, tag);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{id}/translation")]
        public async Task<IActionResult> ChangeTranslationAsync([FromRoute] long id, [FromBody] string translation)
        {
            var response = await _service.ChangeTranslationAsync(id, translation);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{id}/phonetic")]
        public async Task<IActionResult> ChangePhoneticAsync([FromRoute] long id, [FromBody] string phonetic)
        {
            var response = await _service.ChangePhoneticAsync(id, phonetic);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{id}/type")]
        public async Task<IActionResult> ChangeTypeAsync([FromRoute] long id, [FromQuery] int type)
        {
            var response = await _service.ChangeTypeAsync(id, type);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{id}/user-id")]
        public async Task<IActionResult> ChangeUserAsync([FromRoute] long id, [FromBody] string userId)
        {
            var response = await _service.ChangeUserAsync(id, userId);
            return await response.ChangeActionAsync();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] long id)
        {
            var response = await _service.DeleteAsync(id);
            return await response.ChangeActionAsync();
        }

    }
}
