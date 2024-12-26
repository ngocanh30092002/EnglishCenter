using EnglishCenter.Business.IServices;
using EnglishCenter.Presentation.Global;
using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnglishCenter.Presentation.Controllers.AssignmentPage
{
    [Route("api/rc-sentence")]
    [ApiController]
    [Authorize]
    public class RcSentencesController : ControllerBase
    {
        private readonly IQuesRcSentenceService _quesService;
        private readonly IAnswerRcSentenceService _answerService;

        public RcSentencesController(IQuesRcSentenceService quesService, IAnswerRcSentenceService answerService)
        {
            _quesService = quesService;
            _answerService = answerService;
        }

        #region Question

        [HttpGet]
        public async Task<IActionResult> GetQuesSentencesAsync()
        {
            var response = await _quesService.GetAllAsync();
            return await response.ChangeActionAsync();
        }

        [HttpGet("assignments/{id}/other")]
        public async Task<IActionResult> GetOtherQuestionByAssignmentAsync([FromRoute] long id)
        {
            var response = await _quesService.GetOtherQuestionByAssignmentAsync(id);
            return await response.ChangeActionAsync();
        }

        [HttpGet("homework/{id}/other")]
        public async Task<IActionResult> GetOtherQuestionByHomeworkAsync([FromRoute] long id)
        {
            var response = await _quesService.GetOtherQuestionByHomeworkAsync(id);
            return await response.ChangeActionAsync();
        }

        [HttpGet("{quesId}")]
        public async Task<IActionResult> GetQuesSentencesAsync([FromRoute] long quesId)
        {
            var response = await _quesService.GetAsync(quesId);
            return await response.ChangeActionAsync();
        }

        [HttpPost]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> CreateAsync([FromForm] QuesRcSentenceDto queModel)
        {
            var response = await _quesService.CreateAsync(queModel);
            return await response.ChangeActionAsync();
        }

        [HttpPut("{quesId}")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> UpdateAsync([FromRoute] long quesId, [FromForm] QuesRcSentenceDto queModel)
        {
            var response = await _quesService.UpdateAsync(quesId, queModel);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{quesId}/question")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> ChangeQuesQuestionAsync([FromRoute] long quesId, [FromBody] string newQuestion)
        {
            var response = await _quesService.ChangeQuestionAsync(quesId, newQuestion);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{quesId}/answerA")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> ChangeQuesAnswerAAsync([FromRoute] long quesId, [FromBody] string newAnswer)
        {
            var response = await _quesService.ChangeAnswerAAsync(quesId, newAnswer);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{quesId}/answerB")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> ChangeQuesAnswerBAsync([FromRoute] long quesId, [FromBody] string newAnswer)
        {
            var response = await _quesService.ChangeAnswerBAsync(quesId, newAnswer);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{quesId}/answerC")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> ChangeQuesAnswerCAsync([FromRoute] long quesId, [FromBody] string newAnswer)
        {
            var response = await _quesService.ChangeAnswerCAsync(quesId, newAnswer);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{quesId}/answerD")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> ChangeQuesAnswerDAsync([FromRoute] long quesId, [FromBody] string newAnswer)
        {
            var response = await _quesService.ChangeAnswerDAsync(quesId, newAnswer);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{quesId}/answer")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> ChangeQuesAnswerAsync([FromRoute] long quesId, [FromQuery] long answerId)
        {
            var response = await _quesService.ChangeAnswerAsync(quesId, answerId);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{quesId}/time")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> ChangeQuesTimeAsync([FromRoute] long quesId, [FromBody] string time)
        {
            if (!TimeOnly.TryParse(time, out TimeOnly timeOnly))
            {
                var res = new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Time is not in correct format"
                };

                return await res.ChangeActionAsync();
            }
            var response = await _quesService.ChangeTimeAsync(quesId, timeOnly);
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
        public async Task<IActionResult> CreateAnswerAsync([FromForm] AnswerRcSentenceDto model)
        {
            var response = await _answerService.CreateAsync(model);
            return await response.ChangeActionAsync();
        }

        [HttpPut("answers/{answerId}")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> UpdateAnswerAsync([FromRoute] long answerId, [FromForm] AnswerRcSentenceDto model)
        {
            var response = await _answerService.UpdateAsync(answerId, model);
            return await response.ChangeActionAsync();
        }
        [HttpPatch("answers/{answerId}/answerA")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> ChangeAnswerAAsync([FromRoute] long answerId, [FromBody] string newAnswer)
        {
            var response = await _answerService.ChangeAnswerAAsync(answerId, newAnswer);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("answers/{answerId}/answerB")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> ChangeAnswerBAsync([FromRoute] long answerId, [FromBody] string newAnswer)
        {
            var response = await _answerService.ChangeAnswerBAsync(answerId, newAnswer);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("answers/{answerId}/answerC")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> ChangeAnswerCAsync([FromRoute] long answerId, [FromBody] string newAnswer)
        {
            var response = await _answerService.ChangeAnswerCAsync(answerId, newAnswer);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("answers/{answerId}/answerD")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> ChangeAnswerDAsync([FromRoute] long answerId, [FromBody] string newAnswer)
        {
            var response = await _answerService.ChangeAnswerDAsync(answerId, newAnswer);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("answers/{answerId}/correctAnswer")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> ChangeCorrectAnswerAsync([FromRoute] long answerId, [FromBody] string newCorrectAnswer)
        {
            var response = await _answerService.ChangeCorrectAnswerAsync(answerId, newCorrectAnswer);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("answers/{answerId}/explanation")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> ChangeExplanationAsync([FromRoute] long answerId, [FromBody] string newExplanation)
        {
            var response = await _answerService.ChangeExplanationAsync(answerId, newExplanation);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("answers/{answerId}/question")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> ChangeQuestionAsync([FromRoute] long answerId, [FromBody] string newQuestion)
        {
            var response = await _answerService.ChangeQuestionAsync(answerId, newQuestion);
            return await response.ChangeActionAsync();
        }

        [HttpDelete("answers/{answerId}")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> DeleteAnswerAsync([FromRoute] long answerId)
        {
            var response = await _answerService.DeleteAsync(answerId);
            return await response.ChangeActionAsync();
        }
        #endregion
    }
}
