using EnglishCenter.Business.IServices;
using EnglishCenter.Presentation.Global;
using EnglishCenter.Presentation.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnglishCenter.Presentation.Controllers.HomeworkPage
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class HomeQuesController : ControllerBase
    {
        private readonly IHomeQuesService _homeService;

        public HomeQuesController(IHomeQuesService homeService)
        {
            _homeService = homeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var response = await _homeService.GetAllAsync();
            return await response.ChangeActionAsync();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(long id)
        {
            var response = await _homeService.GetAsync(id);
            return await response.ChangeActionAsync();
        }

        [HttpGet("types")]
        public async Task<IActionResult> GetTypeQuesAsync()
        {
            var response = await _homeService.GetTypeQuesAsync();
            return await response.ChangeActionAsync();
        }

        [HttpGet("homework/{homeworkId}")]
        public async Task<IActionResult> GetByHomeworkAsync([FromRoute] long homeworkId)
        {
            var response = await _homeService.GetByHomeworkAsync(homeworkId);
            return await response.ChangeActionAsync();
        }

        [HttpGet("homework/{homeworkId}/{noNum}")]
        public async Task<IActionResult> GetHomeQuesByNoNumAsync([FromRoute] long homeworkId, [FromRoute] int noNum)
        {
            var response = await _homeService.GetHomeQuesByNoNumAsync(homeworkId, noNum);
            return await response.ChangeActionAsync();
        }

        [HttpPost]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> CreateAsync([FromForm] HomeQueDto model)
        {
            var response = await _homeService.CreateAsync(model);
            return await response.ChangeActionAsync();
        }

        [HttpPut("{id}")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> UpdateAsync([FromRoute] long id, [FromForm] HomeQueDto model)
        {
            var response = await _homeService.UpdateAsync(id, model);
            return await response.ChangeActionAsync();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> DeleteAsync([FromRoute] long id)
        {
            var response = await _homeService.DeleteAsync(id);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{id}/change-homework")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> ChangeHomeworkIdAsync([FromRoute] long id, [FromQuery] long homeworkId)
        {
            var response = await _homeService.ChangeHomeworkIdAsync(id, homeworkId);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{id}/change-ques")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> ChangeQuesAsync([FromRoute] long id, [FromQuery] int type, [FromQuery] long quesId)
        {
            var response = await _homeService.ChangeQuesAsync(id, type, quesId);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{id}/change-no-num")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> ChangeNoNumAsync([FromRoute] long id, [FromQuery] int noNum)
        {
            var response = await _homeService.ChangeNoNumAsync(id, noNum);
            return await response.ChangeActionAsync();
        }
    }
}
