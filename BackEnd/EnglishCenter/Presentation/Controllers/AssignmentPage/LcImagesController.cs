using EnglishCenter.Business.IServices;
using EnglishCenter.Presentation.Global;
using EnglishCenter.Presentation.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EnglishCenter.Presentation.Controllers.AssignmentPage
{
    [Route("api/lc-images")]
    [ApiController]
    [Authorize]
    public class LcImagesController : ControllerBase
    {
        private readonly IQuesLcImageService _quesService;
        private readonly IAnswerLcImageService _answerService;

        public LcImagesController(IQuesLcImageService quesService, IAnswerLcImageService answerService)
        {
            _quesService = quesService;
            _answerService = answerService;
        }

        #region Question
        [HttpGet]
        public async Task<IActionResult> GetQuesImagesAsync()
        {
            var response = await _quesService.GetAllAsync();
            return await response.ChangeActionAsync();
        }

        [HttpGet("{quesId}")]
        public async Task<IActionResult> GetQuesImageAsync([FromRoute] long quesId)
        {
            var response = await _quesService.GetAsync(quesId);
            return await response.ChangeActionAsync();
        }

        [HttpPost]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> CreateAsync([FromForm] QuesLcImageDto queModel)
        {
            var response = await _quesService.CreateAsync(queModel);
            return await response.ChangeActionAsync();
        }

        [HttpPut("{quesId}")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> UpdateAsync([FromRoute] long quesId, [FromForm] QuesLcImageDto queModel)
        {
            var response = await _quesService.UpdateAsync(quesId, queModel);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{quesId}/change-answer")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> ChangeAnswerAsync([FromRoute] long quesId, [FromQuery] long answerId)
        {
            var response = await _quesService.ChangeAnswerAsync(quesId, answerId);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{quesId}/change-image")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> ChangeImageAsync([FromRoute] long quesId, IFormFile imageFile)
        {
            var response = await _quesService.ChangeImageAsync(quesId, imageFile);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{quesId}/change-audio")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> ChangeAudioAsync([FromRoute] long quesId, IFormFile audioFile)
        {
            var response = await _quesService.ChangeAudioAsync(quesId, audioFile);
            return await response.ChangeActionAsync();
        }

        [HttpDelete("{quesId}")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> DeleteAsync([FromRoute] long quesId)
        {
            var response = await _quesService.DeleteAsync(quesId);
            return await response.ChangeActionAsync();
        }
        #endregion

        #region Answer
        [HttpGet("answers")]
        public async Task<IActionResult> GetAnswersAsync()
        {
            var response = await _answerService.GetAllAsync();
            return await response.ChangeActionAsync();
        }

        [HttpGet("answers/{answerId}")]
        public async Task<IActionResult> GetAnswerAsync([FromRoute] long answerId)
        {
            var response = await _answerService.GetAsync(answerId);
            return await response.ChangeActionAsync();
        }

        [HttpPost("answers")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> CreateAnswerAsync([FromForm] AnswerLcImageDto model)
        {
            var response = await _answerService.CreateAsync(model);
            return await response.ChangeActionAsync();
        }

        [HttpPut("answers/{answerId}")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> UpdateAnswerAsync([FromRoute] long answerId, [FromForm] AnswerLcImageDto model)
        {
            var response = await _answerService.UpdateAsync(answerId, model);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("answers/{answerId}/change-answerA")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> ChangeAnswerAAsync([FromRoute] long answerId, [FromBody] string newAnswer)
        {
            var response = await _answerService.ChangeAnswerAAsync(answerId, newAnswer);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("answers/{answerId}/change-answerB")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> ChangeAnswerBAsync([FromRoute] long answerId, [FromBody] string newAnswer)
        {
            var response = await _answerService.ChangeAnswerBAsync(answerId, newAnswer);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("answers/{answerId}/change-answerC")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> ChangeAnswerCAsync([FromRoute] long answerId, [FromBody] string newAnswer)
        {
            var response = await _answerService.ChangeAnswerCAsync(answerId, newAnswer);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("answers/{answerId}/change-answerD")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> ChangeAnswerDAsync([FromRoute] long answerId, [FromBody] string newAnswer)
        {
            var response = await _answerService.ChangeAnswerDAsync(answerId, newAnswer);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("answers/{answerId}/change-correctAnswer")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> ChangeCorrectAnswerAsync([FromRoute] long answerId, [FromBody] string newAnswer)
        {
            var response = await _answerService.ChangeCorrectAnswerAsync(answerId, newAnswer);
            return await response.ChangeActionAsync();
        }

        [HttpDelete("answers/{answerId}")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> DeleteAnswerAsync([FromRoute]long answerId)
        {
            var response = await _answerService.DeleteAsync(answerId);
            return await response.ChangeActionAsync();
        }
        #endregion
    }
}
