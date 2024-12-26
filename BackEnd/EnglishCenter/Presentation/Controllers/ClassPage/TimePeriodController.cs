using EnglishCenter.Business.IServices;
using Microsoft.AspNetCore.Mvc;

namespace EnglishCenter.Presentation.Controllers.ClassPage
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimePeriodController : ControllerBase
    {
        private readonly IPeriodService _periodService;

        public TimePeriodController(IPeriodService periodService)
        {
            _periodService = periodService;
        }

        [HttpGet()]
        public async Task<IActionResult> GetAllAsync()
        {
            var response = await _periodService.GetAllAsync();
            return await response.ChangeActionAsync();
        }
    }
}
