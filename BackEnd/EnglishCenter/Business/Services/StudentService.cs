using EnglishCenter.Business.IServices;
using EnglishCenter.DataAccess.UnitOfWork;
using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;
using EnglishCenter.Presentation.Models.ResDTOs;

namespace EnglishCenter.Business.Services
{
    public class StudentService : IStudentService
    {
        private readonly IUnitOfWork _unit;

        public StudentService(IUnitOfWork unit)
        {
            _unit = unit;
        }

        public async Task<Response> ChangeBackgroundImageAsync(IFormFile file, string userId)
        {
            var student = _unit.Students.GetById(userId);

            if (student == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any students",
                    Success = false
                };
            }

            var isSuccess = await _unit.Students.ChangeBackgroundImageAsync(file, student);

            if (!isSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Success = false
                };
            }

            await _unit.CompleteAsync();
            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Success = true,
                Message = ""
            };
        }

        public async Task<Response> ChangePasswordAsync(string userId, string currentPassword, string newPassword)
        {
            var student = _unit.Students.GetById(userId);

            if (student == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any students",
                    Success = false
                };
            }

            var response = await _unit.Students.ChangePasswordAsync(student, currentPassword, newPassword);

            if (response.Success)
            {
                await _unit.CompleteAsync();
            }

            return response;

        }

        public async Task<Response> ChangeStudentBackgroundAsync(string userId, StudentBackgroundDto stuModel)
        {
            var student = _unit.Students.GetById(userId);

            if (student == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any students",
                    Success = false
                };
            }

            var result = _unit.Students.ChangeStudentBackground(student, stuModel);

            if (!result)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Success = false
                };
            }

            await _unit.CompleteAsync();
            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Success = true,
                Message = ""
            };
        }

        public async Task<Response> ChangeStudentImageAsync(IFormFile file, string userId)
        {
            var student = _unit.Students.GetById(userId);

            if (student == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any students",
                    Success = false
                };
            }

            var isSuccess = await _unit.Students.ChangeStudentImageAsync(file, student);

            if (!isSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Success = false
                };
            }

            await _unit.CompleteAsync();
            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Success = true,
                Message = ""
            };
        }

        public async Task<Response> ChangeStudentInfoAsync(string userId, StudentInfoDto model)
        {
            var student = _unit.Students.GetById(userId);

            if (student == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any students",
                    Success = false
                };
            }

            var response = await _unit.Students.ChangeStudentInfoAsync(student, model);

            if (response.Success)
            {
                await _unit.CompleteAsync();
            }

            return response;
        }

        public async Task<Response> GetStudentBackgroundAsync(string userId)
        {
            var student = _unit.Students.GetById(userId);

            if (student == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any students",
                    Success = false
                };
            }

            var result = await _unit.Students.GetStudentBackgroundAsync(student);

            if (result == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Success = false
                };
            }

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = result,
                Success = true
            };
        }

        public async Task<Response> GetStudentInfoAsync(string userId)
        {
            var student = _unit.Students.GetById(userId);

            if (student == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any students",
                    Success = false
                };
            }

            var result = await _unit.Students.GetStudentInfoAsync(student);

            if (result == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Success = false
                };
            }

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = result,
                Success = true
            };
        }

        public async Task<Response> GetFullInfoAsync(string userId)
        {
            var student = _unit.Students.GetById(userId);

            if (student == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any students",
                    Success = false
                };
            }

            var studentBg = await _unit.Students.GetStudentBackgroundAsync(student);
            var studentInfo = await _unit.Students.GetStudentInfoAsync(student);

            if (studentBg == null || studentInfo == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any students info",
                    Success = false
                };
            }

            var result = new StudentResDto()
            {
                FirstName = studentInfo.FirstName,
                LastName = studentInfo.LastName,
                Gender = studentInfo.Gender,
                DateOfBirth = studentInfo.DateOfBirth,
                PhoneNumber = studentInfo.PhoneNumber,
                Email = studentInfo.Email,
                Address = studentInfo.Address,
                UserName = studentInfo.UserName,
                Description = studentBg.Description,
                Roles = studentBg.Roles,
                Image = studentBg.Image,
                BackgroundImage = studentBg.BackgroundImage,
            };


            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = result,
                Success = true
            };
        }
    }
}
