﻿using System.Security.Claims;
using EnglishCenter.Business.IServices;
using EnglishCenter.Presentation.Global;
using EnglishCenter.Presentation.Global.Enum;
using EnglishCenter.Presentation.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnglishCenter.Presentation.Controllers.CoursePage
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EnrollsController : ControllerBase
    {
        private readonly IEnrollmentService _enrollService;
        private readonly IClassService _classService;
        private readonly IRoleService _roleService;

        public EnrollsController(IEnrollmentService enrollService, IClassService classService, IRoleService roleService)
        {
            _enrollService = enrollService;
            _classService = classService;
            _roleService = roleService;
        }

        [HttpGet]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> GetAllAsync()
        {
            var response = await _enrollService.GetAllAsync();
            return await response.ChangeActionAsync();
        }

        [HttpGet("{enrollmentId}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] long enrollmentId)
        {
            var response = await _enrollService.GetAsync(enrollmentId);
            return await response.ChangeActionAsync();
        }

        [HttpGet("student/{userId}")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> GetByStudentAsync([FromRoute] string userId)
        {
            var response = await _enrollService.GetAsync(userId);

            return await response.ChangeActionAsync();
        }

        [HttpGet("student")]
        public async Task<IActionResult> GetByStudentAsync()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest();
            }

            var response = await _enrollService.GetAsync(userId);
            return await response.ChangeActionAsync();
        }

        [HttpGet("{enrollId}/student")]
        public async Task<IActionResult> GetEnrollmentByStudentAsync([FromRoute] long enrollId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";
            var teacher = User.IsInRole(AppRole.TEACHER);
            var admin = User.IsInRole(AppRole.ADMIN);
            var isStudent = !(teacher || admin);

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest();
            }

            var response = await _enrollService.GetAsync(userId, enrollId, isStudent);
            return await response.ChangeActionAsync();
        }

        [HttpGet("student/current-class")]
        public async Task<IActionResult> GetCurrentByStudentAsync()
        {
            var useId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";
            if (string.IsNullOrEmpty(useId))
            {
                return BadRequest();
            }

            var response = await _enrollService.GetCurrentClassesByStudentAsync(useId);
            return await response.ChangeActionAsync();
        }

        [HttpGet("class/{classId}")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> GetByClassAsync([FromRoute] string classId)
        {
            var response = await _enrollService.GetByClassAsync(classId);
            return await response.ChangeActionAsync();
        }

        [HttpGet("class/{classId}/status")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> GetByClassWithStatusAsync([FromRoute] string classId, [FromQuery] int statusId)
        {
            if (!Enum.IsDefined(typeof(EnrollEnum), statusId))
            {
                return BadRequest(new
                {
                    Message = "Status is invalid",
                    Success = false
                });
            }

            var response = await _enrollService.GetAsync(classId, statusId);
            return await response.ChangeActionAsync();
        }

        [HttpGet("teacher/{userId}")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> GetByTeacherAsync([FromRoute] string userId)
        {
            var rolesResponse = await _roleService.GetUserRolesAsync(userId);
            if (rolesResponse.Success)
            {
                var roles = rolesResponse.Message as List<string>;
                if (!roles.Any(r => r == AppRole.TEACHER))
                {
                    return BadRequest(new
                    {
                        Message = "This isn't a teacher",
                        Success = false
                    });
                }
            }

            var response = await _enrollService.GetByTeacherAsync(userId);
            return await response.ChangeActionAsync();
        }

        [HttpGet("teacher")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> GetByTeacherAsync()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest();
            }

            var response = await _enrollService.GetByTeacherAsync(userId);
            return await response.ChangeActionAsync();
        }

        [HttpGet("teacher/{userId}/class/{classId}")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> GetByTeacherAsync([FromRoute] string userId, [FromRoute] string classId)
        {
            var isTeacher = User.IsInRole(AppRole.TEACHER);
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

            if (isTeacher)
            {
                if (currentUserId != userId)
                {
                    return Forbid();
                }

                var isCorrectClass = await _classService.IsClassOfTeacherAsync(userId, classId);
                if (!isCorrectClass)
                {
                    return BadRequest(new
                    {
                        Message = "Classes that the teacher isn't in charge of aren't accessible",
                        Success = false
                    });
                }
            }

            var response = await _enrollService.GetByTeacherAsync(userId, classId);
            return await response.ChangeActionAsync();
        }

        [HttpGet("teacher/class/{classId}")]
        public async Task<IActionResult> GetTeacherWithoutUserIdAsync([FromRoute] string classId)
        {
            var isTeacher = User.IsInRole(AppRole.TEACHER);
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

            if (isTeacher)
            {
                var isCorrectClass = await _classService.IsClassOfTeacherAsync(currentUserId, classId);
                if (!isCorrectClass)
                {
                    return BadRequest(new
                    {
                        Message = "Classes that the teacher isn't in charge of aren't accessible",
                        Success = false
                    });
                }
            }

            var response = await _enrollService.GetByTeacherAsync(currentUserId, classId);
            return await response.ChangeActionAsync();
        }

        [HttpGet("courses/{courseId}")]
        public async Task<IActionResult> GetByCourseAsync([FromRoute] string courseId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest();
            }

            var response = await _enrollService.GetByCourseAsync(userId, courseId);

            return await response.ChangeActionAsync();
        }

        [HttpGet("his/courses/{courseId}")]
        public async Task<IActionResult> GetHisEnrollsByCourseAsync([FromRoute] string courseId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest();
            }

            var response = await _enrollService.GetHisEnrollsByCourseAsync(userId, courseId);

            return await response.ChangeActionAsync();
        }
        [HttpPost]
        [Authorize(Policy = GlobalVariable.ADMIN_STUDENT)]
        public async Task<IActionResult> CreateAsync([FromForm] EnrollmentDto model)
        {
            if (model.UserId == null)
            {
                model.UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";
            }

            var response = await _enrollService.CreateAsync(model);
            return await response.ChangeActionAsync();
        }

        [HttpDelete("{enrollmentId}/remove")]
        public async Task<IActionResult> DeleteByOwnAsync([FromRoute] long enrollmentId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";
            if (string.IsNullOrEmpty(userId)) return BadRequest();

            var response = await _enrollService.DeleteByOwnAsync(enrollmentId, userId);
            return await response.ChangeActionAsync();
        }

        [HttpDelete("{enrollmentId}")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> DeleteAsync([FromRoute] long enrollmentId)
        {
            var response = await _enrollService.DeleteAsync(enrollmentId);
            return await response.ChangeActionAsync();
        }

        [HttpPut("{enrollmentId}")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> UpdateAsync([FromRoute] long enrollmentId, [FromForm] EnrollmentDto model)
        {
            var response = await _enrollService.UpdateAsync(enrollmentId, model);
            return await response.ChangeActionAsync();
        }

        [HttpPut("class/{classId}/handle-start")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> HandleStartClassAsync([FromRoute] string classId)
        {
            var isTeacher = User.IsInRole(AppRole.TEACHER);
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

            if (isTeacher)
            {
                var isClassOfTeacher = await _classService.IsClassOfTeacherAsync(userId, classId);
                if (!isClassOfTeacher)
                {
                    return BadRequest(new
                    {
                        Message = "Classes that the teacher isn't in charge of aren't accessible",
                        Success = false
                    });
                }
            }

            var response = await _enrollService.HandleStartClassAsync(classId);

            return await response.ChangeActionAsync();
        }

        [HttpPut("class/{classId}/handle-end")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> HandleEndClassAsync([FromRoute] string classId)
        {
            var isTeacher = User.IsInRole(AppRole.TEACHER);
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

            if (isTeacher)
            {
                var isClassOfTeacher = await _classService.IsClassOfTeacherAsync(userId, classId);
                if (!isClassOfTeacher)
                {
                    return BadRequest(new
                    {
                        Message = "Classes that the teacher isn't in charge of aren't accessible",
                        Success = false
                    });
                }
            }

            var response = await _enrollService.HandleEndClassAsync(classId);

            return await response.ChangeActionAsync();
        }

        [HttpPut("{enrollmentId}/accept")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> HandleAcceptedAsync([FromRoute] long enrollmentId)
        {
            var isTeacher = User.IsInRole(AppRole.TEACHER);
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

            if (isTeacher)
            {
                var getResponse = await _enrollService.GetAsync(enrollmentId);
                if (getResponse.Success == false)
                {
                    return await getResponse.ChangeActionAsync();
                }

                var enrollModel = getResponse.Message as EnrollmentDto;

                var isClassOfTeacher = await _classService.IsClassOfTeacherAsync(userId, enrollModel?.ClassId);
                if (!isClassOfTeacher)
                {
                    return BadRequest(new
                    {
                        Message = "Classes that the teacher isn't in charge of aren't accessible",
                        Success = false
                    });
                }
            }

            var response = await _enrollService.HandleAcceptedAsync(enrollmentId);
            return await response.ChangeActionAsync();
        }

        [HttpPut("class/{classId}/accept")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> HandleAcceptedAsync([FromRoute] string classId)
        {
            var isTeacher = User.IsInRole(AppRole.TEACHER);
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (isTeacher)
            {
                var isClassOfTeacher = await _classService.IsClassOfTeacherAsync(userId, classId);
                if (!isClassOfTeacher)
                {
                    return BadRequest(new
                    {
                        Message = "Classes that the teacher isn't in charge of aren't accessible",
                        Success = false
                    });
                }
            }

            var response = await _enrollService.HandleAcceptedAsync(classId);

            return await response.ChangeActionAsync();
        }

        [HttpPut("{enrollmentId}/reject")]
        [Authorize(Policy = GlobalVariable.ADMIN_STUDENT)]
        public async Task<IActionResult> HandleRejectAsync(long enrollmentId)
        {
            var isStudent = !(User.IsInRole(AppRole.TEACHER) || User.IsInRole(AppRole.ADMIN));
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

            if (isStudent)
            {
                var isValid = await _enrollService.IsEnrollmentOfStudentAsync(userId, enrollmentId);
                if (!isValid)
                {
                    return BadRequest(new
                    {
                        Message = "Can't reject records that do not belong to you",
                        Sucess = false
                    });
                }
            }

            var response = await _enrollService.HandleRejectAsync(enrollmentId);
            return await response.ChangeActionAsync();
        }

        [HttpPut("{enrollmentId}/teacher/reject")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> HandleRejectByTeacherAsync(long enrollmentId)
        {
            var isTeacher = User.IsInRole(AppRole.TEACHER);
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

            if (isTeacher)
            {
                var getResponse = await _enrollService.GetAsync(enrollmentId);
                if (getResponse.Success == false)
                {
                    return await getResponse.ChangeActionAsync();
                }

                var enrollModel = getResponse.Message as EnrollmentDto;
                var isValid = await _classService.IsClassOfTeacherAsync(userId, enrollModel.ClassId);
                if (!isValid)
                {
                    return BadRequest(new
                    {
                        Message = "Recordings of classes you aren't in charge of can't be rejected",
                        Success = false
                    });
                }
            }

            var response = await _enrollService.HandleRejectByTeacherAsync(enrollmentId);
            return await response.ChangeActionAsync();
        }

        [HttpPut("{enrollmentId}/class/{classId}/change-class")]
        [Authorize(Policy = GlobalVariable.ADMIN_STUDENT)]
        public async Task<IActionResult> HandleChangeClassAsync([FromRoute] long enrollmentId, string classId)
        {
            var isStudent = User.IsInRole(AppRole.STUDENT);
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

            if (isStudent)
            {
                var isValid = await _enrollService.IsEnrollmentOfStudentAsync(userId, enrollmentId);
                if (!isValid)
                {
                    return BadRequest(new
                    {
                        Message = "Can't reject records that do not belong to you",
                        Sucess = false
                    });
                }
            }

            var response = await _enrollService.HandleChangeClassAsync(enrollmentId, classId);
            return await response.ChangeActionAsync();
        }
    }
}
