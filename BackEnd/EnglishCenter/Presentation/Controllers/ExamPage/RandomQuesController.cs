using EnglishCenter.Business.IServices;
using EnglishCenter.Presentation.Global;
using EnglishCenter.Presentation.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnglishCenter.Presentation.Controllers.ExamPage
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RandomQuesController : ControllerBase
    {
        private readonly IRandomQueToeicService _quesService;

        public RandomQuesController(IRandomQueToeicService quesService)
        {
            _quesService = quesService;
        }

        [HttpGet]
        public async Task<IActionResult> GetQuesToeicAsync()
        {
            var response = await _quesService.GetAllAsync();
            return await response.ChangeActionAsync();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync([FromRoute] long id)
        {
            var response = await _quesService.GetAsync(id);
            return await response.ChangeActionAsync();
        }

        [HttpGet("homework/{homeworkId}")]
        public async Task<IActionResult> GetByHomeworkAsync([FromRoute] long homeworkId)
        {
            var response = await _quesService.GetByHomeworkAsync(homeworkId);
            return await response.ChangeActionAsync();
        }

        [HttpGet("homework/{homeworkId}/default")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> GetByDefaultHwAsync([FromRoute] long homeworkId)
        {
            var response = await _quesService.GetByDefaultHwAsync(homeworkId);
            return await response.ChangeActionAsync();
        }

        [HttpGet("homework/{homeworkId}/num-ques")]
        public async Task<IActionResult> GetNumberQuesByHwAsync([FromRoute] long homeworkId)
        {
            var response = await _quesService.GetNumberQuesByHwAsync(homeworkId);
            return await response.ChangeActionAsync();
        }

        [HttpGet("homework/{homeworkId}/num")]
        public async Task<IActionResult> GetNumberByHwAsync([FromRoute] long homeworkId)
        {
            var response = await _quesService.GetNumberByHwAsync(homeworkId);
            return await response.ChangeActionAsync();
        }

        [HttpGet("road-map-exams/{examId}")]
        public async Task<IActionResult> GetByRoadMapExamAsync([FromRoute] long examId)
        {
            var response = await _quesService.GetByRoadMapExamAsync(examId);
            return await response.ChangeActionAsync();
        }

        [HttpGet("road-map-exams/{examId}/default")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> GetByDefaultRmAsync([FromRoute] long examId)
        {
            var response = await _quesService.GetByDefaultRmAsync(examId);
            return await response.ChangeActionAsync();
        }

        [HttpGet("road-map-exams/{roadMapExamId}/num-ques")]
        public async Task<IActionResult> GetNumberQuesByRmAsync([FromRoute] long roadMapExamId)
        {
            var response = await _quesService.GetNumberQuesByRmAsync(roadMapExamId);
            return await response.ChangeActionAsync();
        }

        [HttpGet("road-map-exams/{roadMapExamId}/num")]
        public async Task<IActionResult> GetNumberByRmAsync([FromRoute] long roadMapExamId)
        {
            var response = await _quesService.GetNumberByRmAsync(roadMapExamId);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{id}/homework")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> ChangeHomeworkAsync([FromRoute] long id, [FromQuery] int homeworkId)
        {
            var response = await _quesService.ChangeHomeworkAsync(id, homeworkId);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{id}/ques")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> ChangeQuesToeicAsync([FromRoute] long id, [FromQuery] int quesId)
        {
            var response = await _quesService.ChangeQuesToeicAsync(id, quesId);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{id}/road-map-exams")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> ChangeRoadMapAsync([FromRoute] long id, [FromQuery] int quesId)
        {
            var response = await _quesService.ChangeRoadMapAsync(id, quesId);
            return await response.ChangeActionAsync();
        }

        [HttpPost]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> CreateAsync([FromForm] RandomQuesToeicDto model)
        {
            var response = await _quesService.CreateAsync(model);
            return await response.ChangeActionAsync();
        }

        [HttpPost("homework/random")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> HandleCreateHwWithLevelAsync([FromForm] RandomPartDto model)
        {
            var response = await _quesService.HandleCreateHwWithLevelAsync(model);
            return await response.ChangeActionAsync();
        }

        [HttpPost("homework/{homeworkId}/random/list")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> HandleCreateHwWithLevelAsync([FromRoute] long homeworkId, [FromForm] List<RandomPartWithLevelDto> models)
        {
            var response = await _quesService.HandleCreateHwWithLevelAsync(homeworkId, models);
            return await response.ChangeActionAsync();
        }

        [HttpPost("road-map-exams/random")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> HandleCreateRmWithLevelAsync([FromForm] RandomPartDto model)
        {
            var response = await _quesService.HandleCreateRmWithLevelAsync(model);
            return await response.ChangeActionAsync();
        }

        [HttpPost("road-map-exams/{examId}/random/list")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> HandleCreateRmWithLevelAsync([FromRoute] long examId, [FromForm] List<RandomPartWithLevelDto> models)
        {
            var response = await _quesService.HandleCreateRmWithLevelAsync(examId, models);
            return await response.ChangeActionAsync();
        }

        [HttpPost("road-map-exams/list")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> HandleCreateRmByListAsync([FromForm] List<RandomQuesToeicDto> models)
        {
            var response = await _quesService.HandleCreateRmByListAsync(models);
            return await response.ChangeActionAsync();
        }

        [HttpPost("homework/list")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> HandleCreateHwByListAsync([FromForm] List<RandomQuesToeicDto> models)
        {
            var response = await _quesService.HandleCreateHwByListAsync(models);
            return await response.ChangeActionAsync();
        }

        [HttpPut("{id}")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> UpdateAsync([FromRoute] long id, [FromForm] RandomQuesToeicDto model)
        {
            var response = await _quesService.UpdateAsync(id, model);
            return await response.ChangeActionAsync();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> DeleteAsync([FromRoute] long id)
        {
            var response = await _quesService.DeleteAsync(id);
            return await response.ChangeActionAsync();
        }

        [HttpDelete("homework/{homeworkId}")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> DeleteHwAsync([FromRoute] long homeworkId, [FromQuery] long quesId)
        {
            var response = await _quesService.DeleteHwAsync(homeworkId, quesId);
            return await response.ChangeActionAsync();
        }

        [HttpDelete("road-map-exams/{roadmapId}")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> DeleteRmAsync([FromRoute] long roadmapId, [FromQuery] long quesId)
        {
            var response = await _quesService.DeleteRmAsync(roadmapId, quesId);
            return await response.ChangeActionAsync();
        }
    }
}
