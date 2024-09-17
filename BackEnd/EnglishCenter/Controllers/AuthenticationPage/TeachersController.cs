using EnglishCenter.Repositories.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EnglishCenter.Controllers.AuthenticationPage
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeachersController : ControllerBase
    {
        private readonly ITeacherRepository _teacherRepo;

        public TeachersController(ITeacherRepository teacherRepo)
        {
            _teacherRepo = teacherRepo;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetFullNameAsync([FromRoute]string userId)
        {
            var response = await _teacherRepo.GetFullNameAsync(userId);
            return await response.ChangeActionAsync();
        }
    }
}
