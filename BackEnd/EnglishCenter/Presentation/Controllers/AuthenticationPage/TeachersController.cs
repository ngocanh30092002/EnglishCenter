using EnglishCenter.Business.IServices;
using Microsoft.AspNetCore.Mvc;

namespace EnglishCenter.Presentation.Controllers.AuthenticationPage
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeachersController : ControllerBase
    {
        private readonly ITeacherService _teacherService;

        public TeachersController(ITeacherService teacherService)
        {
            _teacherService = teacherService;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetFullNameAsync([FromRoute] string userId)
        {
            var response = _teacherService.GetFullName(userId);

            return await response.ChangeActionAsync();
        }
    }
}
