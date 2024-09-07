using AutoMapper;
using EnglishCenter.Database;
using EnglishCenter.Models;
using EnglishCenter.Models.DTO;
using EnglishCenter.Repositories.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EnglishCenter.Controllers.CoursePage
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICourseRepository _courseRepo;

        public CoursesController(ICourseRepository courseRepo ,IMapper mapper)
        {
            _mapper = mapper;
            _courseRepo = courseRepo;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetCoursesAsync()
        {
            var courses = await _courseRepo.GetCoursesAsync();

            var coursesDto = _mapper.Map<List<CourseDtoModel>>(courses);

            return Ok(coursesDto);
        }

        [HttpGet("{courseId}")]
        public async Task<IActionResult> GetCourseAsync([FromRoute] string courseId)
        {
            var course = await _courseRepo.GetCourseAsync(courseId);

            if (course == null)
            {
                return BadRequest(new
                {
                    Message = "Couldn't find any courses"
                });
            }

            var courseDto = _mapper.Map<CourseDtoModel>(course);

            return Ok(courseDto);
        }

        [HttpPost("")]
        public async Task<IActionResult> CreateCourseAsync([FromBody] CourseDtoModel model)
        {
            var courseInfo = _mapper.Map<Course>(model);

            var response = await _courseRepo.CreateCourseAsync(courseInfo);

            return await response.ChangeActionAsync();
        }

        [HttpPut("{courseId}")]
        public async Task<IActionResult> UpdateCourseAsync([FromRoute] string courseId, [FromBody] CourseDtoModel model)
        {
            var courseInfo = _mapper.Map<Course>(model);

            var response = await _courseRepo.UpdateCourseAsync(courseId, courseInfo);

            return await response.ChangeActionAsync();
        }

        [HttpDelete("{courseId}")]
        public async Task<IActionResult> DeleteCourseAsync([FromRoute] string courseId)
        {
            var response = await _courseRepo.DeleteCourseAsync(courseId);

            return await response.ChangeActionAsync();
        }

        [HttpPatch("{courseId}/priority/{priority}")]
        public async Task<IActionResult> ChangePriorityAsync([FromRoute] string courseId, [FromRoute] int priority)
        {
            var response = await _courseRepo.ChangePriorityAsync(courseId, priority);

            return await response.ChangeActionAsync();
        }
    }
}
