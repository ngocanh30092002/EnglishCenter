using EnglishCenter.Business.IServices;
using EnglishCenter.Presentation.Global;
using EnglishCenter.Presentation.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EnglishCenter.Presentation.Controllers.AssignmentPage
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AnswerRecordsController : ControllerBase
    {
        private readonly IAnswerRecordService _answerService;

        public AnswerRecordsController(IAnswerRecordService answerService)
        {
            _answerService = answerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var response = await _answerService.GetAllAsync();
            return await response.ChangeActionAsync();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync([FromRoute] long id)
        {
            var response = await _answerService.GetAsync(id);
            return await response.ChangeActionAsync();  
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromForm] AnswerRecordDto model)
        {
            var response = await _answerService.CreateAsync(model);
            return await response.ChangeActionAsync();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> UpdateAsync([FromRoute]long id, [FromForm] AnswerRecordDto model)
        {
            var response = await _answerService.UpdateAsync(id, model);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{id}/process")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> ChangeProcessAsync([FromRoute] long id, [FromQuery] long processId)
        {
            var response = await _answerService.ChangeProcessAsync(id, processId);
            return await response.ChangeActionAsync();
        }


        [HttpPatch("{id}/assign-ques")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> ChangeAssignQuesAsync([FromRoute] long id, [FromQuery] long assignQueId)
        {
            var response = await _answerService.ChangeAssignQuesAsync(id, assignQueId);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{id}/selected-answer")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> ChangeSelectedAnswerAsync([FromRoute] long id, [FromQuery] string selectedAnswer)
        {
            var response = await _answerService.ChangeSelectedAnswerAsync(id, selectedAnswer);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{id}/sub")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> ChangeSubAsync([FromRoute] long id, [FromQuery] long? subId)
        {
            var response = await _answerService.ChangeSubAsync(id, subId);
            return await response.ChangeActionAsync();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> DeleteAsync([FromRoute] long id)
        {
            var response = await _answerService.DeleteAsync(id);
            return await response.ChangeActionAsync();
        }
    }
}
