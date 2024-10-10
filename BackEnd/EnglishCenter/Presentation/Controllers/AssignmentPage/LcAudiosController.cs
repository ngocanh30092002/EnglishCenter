using EnglishCenter.Business.IServices;
using EnglishCenter.Presentation.Global;
using EnglishCenter.Presentation.Helpers;
using EnglishCenter.Presentation.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EnglishCenter.Presentation.Controllers.AssignmentPage
{
    [Route("api/lc-audios")]
    [ApiController]
    [Authorize]
    public class LcAudiosController : ControllerBase
    {
        private readonly IQuesLcAudioService _audioService;
        private readonly IAnswerLcAudioService _answerService;

        public LcAudiosController(IQuesLcAudioService audioService, IAnswerLcAudioService answerService)
        {
            _audioService = audioService;
            _answerService = answerService;
        }

        #region Question
        [HttpGet]
        public async Task<IActionResult> GetQuesAudiosAsync()
        {
            var response = await _audioService.GetAllAsync();
            return await response.ChangeActionAsync();
        }

        [HttpGet("{quesId}")]
        public async Task<IActionResult> GetQuesAudioAsync([FromRoute] long quesId)
        {
            var response = await _audioService.GetAsync(quesId);
            return await response.ChangeActionAsync();
        }

        [HttpPost]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> CreateAsync([FromForm] QuesLcAudioDto queModel)
        {
            if(queModel.Audio != null)
            {
                var isAudioFile = await UploadHelper.IsAudioAsync(queModel.Audio);
                if (!isAudioFile)
                {
                    return BadRequest(new { message = "The audio file is invalid. Only MP3, WAV and OGG are allowed. ", success = false });
                }
            }

            var response = await _audioService.CreateAsync(queModel);
            return await response.ChangeActionAsync();
        }

        [HttpPut("{quesId}")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> UpdateAsync([FromRoute] long quesId, [FromForm] QuesLcAudioDto queModel)
        {
            if (queModel.Audio != null)
            {
                var isAudioFile = await UploadHelper.IsAudioAsync(queModel.Audio);
                if (!isAudioFile)
                {
                    return BadRequest(new { message = "The audio file is invalid. Only MP3, WAV and OGG are allowed. ", success = false });
                }
            }

            var response = await _audioService.UpdateAsync(quesId, queModel);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{quesId}/change-answer")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> ChangeAnswerAsync([FromRoute] long quesId, [FromQuery] long answerId)
        {
            var response = await _audioService.ChangeAnswerAsync(quesId, answerId);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{quesId}/change-answerA")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> ChangeQuesAnswerAAsync([FromRoute] long quesId, [FromBody]string newAnswer)
        {
            var response = await _audioService.ChangeAnswerAAsync(quesId, newAnswer);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{quesId}/change-answerB")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> ChangeQuesAnswerBAsync([FromRoute] long quesId, [FromBody] string newAnswer)
        {
            var response = await _audioService.ChangeAnswerBAsync(quesId, newAnswer);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{quesId}/change-answerC")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> ChangeQuesAnswerCAsync([FromRoute] long quesId, [FromBody] string newAnswer)
        {
            var response = await _audioService.ChangeAnswerCAsync(quesId, newAnswer);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{quesId}/change-question")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> ChangeQuesQuestionAsync([FromRoute] long quesId, [FromBody] string newQues)
        {
            var response = await _audioService.ChangeQuestionAsync(quesId, newQues);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{quesId}/change-audio")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> ChangeAudioAsync([FromRoute] long quesId, IFormFile audioFile)
        {
            var isAudioFile = await UploadHelper.IsAudioAsync(audioFile);
            if (!isAudioFile)
            {
                return BadRequest(new { message = "The audio file is invalid. Only MP3, WAV and OGG are allowed. ", success = false });
            }
            
            var response = await _audioService.ChangeAudioAsync(quesId, audioFile);
            return await response.ChangeActionAsync();
        }

        [HttpDelete("{quesId}")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> DeleteAsync([FromRoute] long quesId)
        {
            var response = await _audioService.DeleteAsync(quesId);
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
        public async Task<IActionResult> CreateAnswerAsync([FromForm] AnswerLcAudioDto model)
        {
            var response = await _answerService.CreateAsync(model);
            return await response.ChangeActionAsync();
        }

        [HttpPut("answers/{answerId}")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> UpdateAnswerAsync([FromRoute] long answerId, [FromForm] AnswerLcAudioDto model)
        {
            var response = await _answerService.UpdateAsync(answerId, model);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("answers/{answerId}/change-question")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> ChangeQuestionAsync([FromRoute] long answerId, [FromBody] string newQues)
        {
            var response = await _answerService.ChangeQuestionAsync(answerId, newQues);
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

        [HttpPatch("answers/{answerId}/change-correctAnswer")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> ChangeCorrectAnswerAsync([FromRoute] long answerId, [FromBody] string newAnswer)
        {
            var response = await _answerService.ChangeCorrectAnswerAsync(answerId, newAnswer);
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
