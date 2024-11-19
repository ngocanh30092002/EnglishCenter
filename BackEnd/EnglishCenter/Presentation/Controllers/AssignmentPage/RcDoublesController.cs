using EnglishCenter.Business.IServices;
using EnglishCenter.Business.Services.Assignments;
using EnglishCenter.Presentation.Global;
using EnglishCenter.Presentation.Helpers;
using EnglishCenter.Presentation.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EnglishCenter.Presentation.Controllers.AssignmentPage
{
    [Route("api/rc-double")]
    [ApiController]
    public class RcDoublesController : ControllerBase
    {
        private readonly IQuesRcDoubleService _quesService;
        private readonly ISubRcDoubleService _subService;
        private readonly IAnswerRcDoubleService _answerService;

        public RcDoublesController(IQuesRcDoubleService quesService, ISubRcDoubleService subService, IAnswerRcDoubleService answerService)
        {
            _quesService = quesService;
            _subService = subService;
            _answerService = answerService;
        }

        #region Question

        [HttpGet]
        public async Task<IActionResult> GetQuesDoubleAsync()
        {
            var response = await _quesService.GetAllAsync();
            return await response.ChangeActionAsync();
        }

        [HttpGet("{quesId}")]
        public async Task<IActionResult> GetQuesDoubleAsync([FromRoute] long quesId)
        {
            var response = await _quesService.GetAsync(quesId);
            return await response.ChangeActionAsync();
        }

        [HttpPost]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> CreateAsync([FromForm] QuesRcDoubleDto queModel)
        {
            if (queModel.Image1 != null)
            {
                var isImageFile = await UploadHelper.IsImageAsync(queModel.Image1);
                if (!isImageFile)
                {
                    return BadRequest(new { message = "The image file is invalid. Only JPEG, PNG, GIF, and SVG are allowed.", success = false });
                }
            }

            if (queModel.Image2 != null)
            {
                var isImageFile = await UploadHelper.IsImageAsync(queModel.Image2);
                if (!isImageFile)
                {
                    return BadRequest(new { message = "The image file is invalid. Only JPEG, PNG, GIF, and SVG are allowed.", success = false });
                }
            }

            var response = await _quesService.CreateAsync(queModel);
            return await response.ChangeActionAsync();
        }

        [HttpPut("{quesId}")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> UpdateAsync([FromRoute] long quesId, [FromForm] QuesRcDoubleDto queModel)
        {
            if (queModel.Image1 != null)
            {
                var isImageFile = await UploadHelper.IsImageAsync(queModel.Image1);
                if (!isImageFile)
                {
                    return BadRequest(new { message = "The image file is invalid. Only JPEG, PNG, GIF, and SVG are allowed.", success = false });
                }
            }

            if (queModel.Image2 != null)
            {
                var isImageFile = await UploadHelper.IsImageAsync(queModel.Image2);
                if (!isImageFile)
                {
                    return BadRequest(new { message = "The image file is invalid. Only JPEG, PNG, GIF, and SVG are allowed.", success = false });
                }
            }

            var response = await _quesService.UpdateAsync(quesId, queModel);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{quesId}/image1")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> ChangeImage1Async([FromRoute] long quesId, IFormFile imageFile)
        {
            var isImageFile = await UploadHelper.IsImageAsync(imageFile);
            if (!isImageFile)
            {
                return BadRequest(new { message = "The image file is invalid. Only JPEG, PNG, GIF, and SVG are allowed.", success = false });
            }

            var response = await _quesService.ChangeImage1Async(quesId, imageFile);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{quesId}/image2")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> ChangeImage2Async([FromRoute] long quesId, IFormFile imageFile)
        {
            var isImageFile = await UploadHelper.IsImageAsync(imageFile);
            if (!isImageFile)
            {
                return BadRequest(new { message = "The image file is invalid. Only JPEG, PNG, GIF, and SVG are allowed.", success = false });
            }

            var response = await _quesService.ChangeImage2Async(quesId, imageFile);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{quesId}/quantity")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> ChangeQuantityAsync([FromRoute] long quesId, [FromQuery] int quantity)
        {
            var response = await _quesService.ChangeQuantityAsync(quesId, quantity);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{quesId}/time")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> ChangeTimeAsync([FromRoute] long quesId, [FromBody] string time)
        {
            if (!TimeOnly.TryParse(time, out TimeOnly timeOnly))
            {
                return BadRequest(new { message = "Time is not in correct format", success = false });
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

        #region SubQuestion

        [HttpGet("subs")]
        public async Task<IActionResult> GetSubsAsync()
        {
            var response = await _subService.GetAllAsync();
            return await response.ChangeActionAsync();
        }

        [HttpGet("subs/{subId}")]
        public async Task<IActionResult> GetSubAsync([FromRoute] long subId)
        {
            var response = await _subService.GetAsync(subId);
            return await response.ChangeActionAsync();
        }

        [HttpPost("subs")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> CreateSubAsync([FromForm] SubRcDoubleDto queModel)
        {
            var response = await _subService.CreateAsync(queModel);
            return await response.ChangeActionAsync();
        }

        [HttpPut("subs/{subId}")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> UpdateAsync([FromRoute] long subId, [FromForm] SubRcDoubleDto queModel)
        {
            var response = await _subService.UpdateAsync(subId, queModel);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("subs/{subId}/answerA")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> ChangeSubAnswerAAsync([FromRoute] long subId, [FromBody] string newAnswer)
        {
            var response = await _subService.ChangeAnswerAAsync(subId, newAnswer);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("subs/{subId}/answerB")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> ChangeSubAnswerBAsync([FromRoute] long subId, [FromBody] string newAnswer)
        {
            var response = await _subService.ChangeAnswerBAsync(subId, newAnswer);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("subs/{subId}/answerC")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> ChangeSubAnswerCAsync([FromRoute] long subId, [FromBody] string newAnswer)
        {
            var response = await _subService.ChangeAnswerCAsync(subId, newAnswer);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("subs/{subId}/answerD")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> ChangeSubAnswerDAsync([FromRoute] long subId, [FromBody] string newAnswer)
        {
            var response = await _subService.ChangeAnswerDAsync(subId, newAnswer);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("subs/{subId}/answer")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> ChangeSubAnswerAsync([FromRoute] long subId, [FromBody] int answerId)
        {
            var response = await _subService.ChangeAnswerAsync(subId, answerId);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("subs/{subId}/question")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> ChangeSubQuestionAsync([FromRoute] long subId, [FromBody] string newQues)
        {
            var response = await _subService.ChangeQuestionAsync(subId, newQues);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("subs/{subId}/no-num")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> ChangeSubNoNumAsync([FromRoute] long subId, [FromBody] int noNum)
        {
            var response = await _subService.ChangeNoNumAsync(subId, noNum);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("subs/{subId}/pre-ques")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> ChangeSubPreQuesAsync([FromRoute] long subId, [FromBody] long preQues)
        {
            var response = await _subService.ChangePreQuesAsync(subId, preQues);
            return await response.ChangeActionAsync();
        }

        [HttpDelete("subs/{subId}")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> DeleteSubAsync([FromRoute] long subId)
        {
            var response = await _subService.DeleteAsync(subId);
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

        [HttpGet("answer/{answerId}")]
        public async Task<IActionResult> GetAnswerAsync([FromRoute] long answerId)
        {
            var response = await _answerService.GetAsync(answerId);
            return await response.ChangeActionAsync();
        }

        [HttpPost("answers")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> CreateAnswerAsync([FromForm] AnswerRcDoubleDto model)
        {
            var response = await _answerService.CreateAsync(model);
            return await response.ChangeActionAsync();
        }

        [HttpPut("answers/{answerId}")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> UpdateAnswerAsync([FromRoute] long answerId, [FromForm] AnswerRcDoubleDto model)
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

        [HttpPatch("answers/{answerId}/question")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> ChangeQuestionAsync([FromRoute] long answerId, [FromBody] string newQues)
        {
            var response = await _answerService.ChangeQuestionAsync(answerId, newQues);
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
