using EnglishCenter.Business.IServices;
using EnglishCenter.Presentation.Global;
using EnglishCenter.Presentation.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnglishCenter.Presentation.Controllers.ClassPage
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassRoomsController : ControllerBase
    {
        private readonly IClassRoomService _classRoomService;

        public ClassRoomsController(IClassRoomService classRoomService)
        {
            _classRoomService = classRoomService;
        }

        [HttpGet()]
        public async Task<IActionResult> GetAllAsync()
        {
            var response = await _classRoomService.GetAllAsync();
            return await response.ChangeActionAsync();
        }

        [HttpGet("{id}/lessons")]
        public async Task<IActionResult> GetLessonsAsync([FromRoute] long id)
        {
            var response = await _classRoomService.GetLessonsAsync(id);
            return await response.ChangeActionAsync();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(long id)
        {
            var response = await _classRoomService.GetAsync(id);
            return await response.ChangeActionAsync();
        }

        [HttpPost]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> CreateAsync([FromForm] ClassRoomDto model)
        {
            var response = await _classRoomService.CreateAsync(model);
            return await response.ChangeActionAsync();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> UpdateAsync([FromRoute] long id, [FromForm] ClassRoomDto model)
        {
            var response = await _classRoomService.UpdateAsync(id, model);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{id}/location")]
        public async Task<IActionResult> ChangeLocationAsync([FromRoute] long id, [FromBody] string location)
        {
            var response = await _classRoomService.ChangeLocationAsync(id, location);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{id}/name")]
        public async Task<IActionResult> ChangeNameAsync([FromRoute] long id, [FromBody] string newName)
        {
            var response = await _classRoomService.ChangeNameAsync(id, newName);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{id}/capacity")]
        public async Task<IActionResult> ChangeCapacityAsync([FromRoute] long id, [FromQuery] int capacity)
        {
            var response = await _classRoomService.ChangeCapacityAsync(id, capacity);
            return await response.ChangeActionAsync();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> DeleteAsync([FromRoute] long id)
        {
            var response = await _classRoomService.DeleteAsync(id);
            return await response.ChangeActionAsync();
        }
    }
}
