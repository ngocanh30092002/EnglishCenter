using EnglishCenter.Business.IServices;
using EnglishCenter.Presentation.Global;
using EnglishCenter.Presentation.Helpers;
using EnglishCenter.Presentation.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace EnglishCenter.Presentation.Controllers.ExamPage
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class QuesToeicController : ControllerBase
    {
        private readonly IQuesToeicService _quesService;
        private readonly ISubToeicService _subService;
        private readonly IAnswerToeicService _answerService;

        public QuesToeicController(IQuesToeicService quesService, ISubToeicService subService, IAnswerToeicService answerService)
        {
            _quesService = quesService;
            _subService = subService;
            _answerService = answerService;
        }

        #region Question
        [HttpGet]
        public async Task<IActionResult> GetQuesToeicAsync()
        {
            var response = await _quesService.GetAllAsync();
            return await response.ChangeActionAsync();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetQuesToeicAsync([FromRoute] long id)
        {
            var response = await _quesService.GetAsync(id);
            return await response.ChangeActionAsync();
        }

        [HttpGet("toeic/{id}")]
        public async Task<IActionResult> GetByToeicAsync([FromRoute] long id)
        {
            var response = await _quesService.GetByToeicAsync(id);
            return await response.ChangeActionAsync();
        }

        [HttpGet("toeic/{id}/num-ques")]
        public async Task<IActionResult> GetTotalNumberSentences([FromRoute] long id)
        {
            var response = await _quesService.GetTotalNumberSentences(id);
            return await response.ChangeActionAsync();
        }

        [HttpPost]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> CreateAsync([FromForm] QuesToeicDto model)
        {
            if (model.Audio != null)
            {
                var isAudioFile = await UploadHelper.IsAudioAsync(model.Audio);
                if (!isAudioFile)
                {
                    return BadRequest(new { message = "The audio file is invalid. Only MP3, WAV and OGG are allowed. ", success = false });
                }
            }

            if (model.Image_1 != null)
            {
                var isImageFile = await UploadHelper.IsImageAsync(model.Image_1);
                if (!isImageFile)
                {
                    return BadRequest(new { message = "The image_1 file is invalid. Only JPEG, PNG, GIF, and SVG are allowed.", success = false });
                }
            }

            if (model.Image_2 != null)
            {
                var isImageFile = await UploadHelper.IsImageAsync(model.Image_2);
                if (!isImageFile)
                {
                    return BadRequest(new { message = "The image_2 file is invalid. Only JPEG, PNG, GIF, and SVG are allowed.", success = false });
                }
            }

            if (model.Image_3 != null)
            {
                var isImageFile = await UploadHelper.IsImageAsync(model.Image_3);
                if (!isImageFile)
                {
                    return BadRequest(new { message = "The image_3 file is invalid. Only JPEG, PNG, GIF, and SVG are allowed.", success = false });
                }
            }

            if (!string.IsNullOrEmpty(model.SubToeicDtoJson))
            {
                model.SubToeicDtos = JsonConvert.DeserializeObject<List<SubToeicDto>>(model.SubToeicDtoJson);
            }

            var response = await _quesService.CreateAsync(model);
            return await response.ChangeActionAsync();
        }

        [HttpPut("{id}")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> UpdateAsync([FromRoute] long id, [FromForm] QuesToeicDto model)
        {
            if (model.Audio != null)
            {
                var isAudioFile = await UploadHelper.IsAudioAsync(model.Audio);
                if (!isAudioFile)
                {
                    return BadRequest(new { message = "The audio file is invalid. Only MP3, WAV and OGG are allowed. ", success = false });
                }
            }

            if (model.Image_1 != null)
            {
                var isImageFile = await UploadHelper.IsImageAsync(model.Image_1);
                if (!isImageFile)
                {
                    return BadRequest(new { message = "The image_1 file is invalid. Only JPEG, PNG, GIF, and SVG are allowed.", success = false });
                }
            }

            if (model.Image_2 != null)
            {
                var isImageFile = await UploadHelper.IsImageAsync(model.Image_2);
                if (!isImageFile)
                {
                    return BadRequest(new { message = "The image_2 file is invalid. Only JPEG, PNG, GIF, and SVG are allowed.", success = false });
                }
            }

            if (model.Image_3 != null)
            {
                var isImageFile = await UploadHelper.IsImageAsync(model.Image_3);
                if (!isImageFile)
                {
                    return BadRequest(new { message = "The image_3 file is invalid. Only JPEG, PNG, GIF, and SVG are allowed.", success = false });
                }
            }

            var response = await _quesService.UpdateAsync(id, model);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{id}/audio")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> ChangeAudioAsync([FromRoute] long id, IFormFile audioFile)
        {
            if (audioFile != null)
            {
                var isAudioFile = await UploadHelper.IsAudioAsync(audioFile);
                if (!isAudioFile)
                {
                    return BadRequest(new { message = "The audio file is invalid. Only MP3, WAV and OGG are allowed. ", success = false });
                }
            }

            var response = await _quesService.ChangeAudioAsync(id, audioFile);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{id}/image-1")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> ChangeImage1Async([FromRoute] long id, IFormFile imageFile)
        {
            if (imageFile != null)
            {
                var isImageFile = await UploadHelper.IsImageAsync(imageFile);
                if (!isImageFile)
                {
                    return BadRequest(new { message = "The image_1 file is invalid. Only JPEG, PNG, GIF, and SVG are allowed.", success = false });
                }
            }

            var response = await _quesService.ChangeImage1Async(id, imageFile);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{id}/image-2")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> ChangeImage2Async([FromRoute] long id, IFormFile imageFile)
        {
            if (imageFile != null)
            {
                var isImageFile = await UploadHelper.IsImageAsync(imageFile);
                if (!isImageFile)
                {
                    return BadRequest(new { message = "The image_2 file is invalid. Only JPEG, PNG, GIF, and SVG are allowed.", success = false });
                }
            }

            var response = await _quesService.ChangeImage2Async(id, imageFile);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{id}/image-3")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> ChangeImage3Async([FromRoute] long id, IFormFile imageFile)
        {
            if (imageFile != null)
            {
                var isImageFile = await UploadHelper.IsImageAsync(imageFile);
                if (!isImageFile)
                {
                    return BadRequest(new { message = "The image_3 file is invalid. Only JPEG, PNG, GIF, and SVG are allowed.", success = false });
                }
            }

            var response = await _quesService.ChangeImage3Async(id, imageFile);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{id}/no-num")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> ChangeNoNumAsync([FromRoute] long id, [FromQuery] int noNum)
        {
            var response = await _quesService.ChangeNoNumAsync(id, noNum);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{id}/group")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> ChangeGroupAsync([FromRoute] long id, [FromQuery] bool isGroup)
        {
            var response = await _quesService.ChangeGroupAsync(id, isGroup);
            return await response.ChangeActionAsync();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] long id)
        {
            var response = await _quesService.DeleteAsync(id);
            return await response.ChangeActionAsync();
        }

        #endregion

        #region SubQuestion

        [HttpGet("subs")]
        public async Task<IActionResult> GetSubQuesAsync()
        {
            var response = await _subService.GetAllAsync();
            return await response.ChangeActionAsync();
        }

        [HttpGet("subs/{id}")]
        public async Task<IActionResult> GetSubQuesAsync([FromRoute] long id)
        {
            var response = await _subService.GetAllAsync();
            return await response.ChangeActionAsync();
        }

        [HttpPost("subs")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> CreateSubAsync([FromForm] SubToeicDto model)
        {
            var response = await _subService.CreateAsync(model);
            return await response.ChangeActionAsync();
        }

        [HttpPut("subs/{id}")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> UpdateSubAsync([FromRoute] long id, [FromForm] SubToeicDto model)
        {
            var response = await _subService.UpdateAsync(id, model);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("subs/{id}/answerA")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> ChangeSubAnswerAAsync([FromRoute] long id, [FromForm] string newAnswer)
        {
            var response = await _subService.ChangeAnswerAAsync(id, newAnswer);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("subs/{id}/answerB")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> ChangeSubAnswerBAsync([FromRoute] long id, [FromForm] string newAnswer)
        {
            var response = await _subService.ChangeAnswerBAsync(id, newAnswer);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("subs/{id}/answerC")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> ChangeSubAnswerCAsync([FromRoute] long id, [FromForm] string newAnswer)
        {
            var response = await _subService.ChangeAnswerCAsync(id, newAnswer);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("subs/{id}/answerD")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> ChangeSubAnswerDAsync([FromRoute] long id, [FromForm] string newAnswer)
        {
            var response = await _subService.ChangeAnswerDAsync(id, newAnswer);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("subs/{id}/answer")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> ChangeSubAnswerAsync([FromRoute] long id, [FromQuery] long answerId)
        {
            var response = await _subService.ChangeAnswerAsync(id, answerId);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("subs/{id}/question")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> ChangeSubQuestionAsync([FromRoute] long id, [FromForm] string question)
        {
            var response = await _subService.ChangeQuestionAsync(id, question);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("subs/{id}/ques-no")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> ChangeSubNoNumAsync([FromRoute] long id, [FromRoute] int quesNo)
        {
            var response = await _subService.ChangeQuesNoAsync(id, quesNo);
            return await response.ChangeActionAsync();
        }

        [HttpDelete("subs/{id}")]
        public async Task<IActionResult> DeleteSubAsync([FromRoute] long id)
        {
            var response = await _subService.DeleteAsync(id);
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
        public async Task<IActionResult> CreateAnswerAsync([FromForm] AnswerToeicDto model)
        {
            var response = await _answerService.CreateAsync(model);
            return await response.ChangeActionAsync();
        }

        [HttpPut("answers/{answerId}")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> UpdateAnswerAsync([FromRoute] long answerId, [FromForm] AnswerToeicDto model)
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
