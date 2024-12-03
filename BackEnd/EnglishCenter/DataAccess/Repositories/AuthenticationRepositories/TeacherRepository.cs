using EnglishCenter.DataAccess.Database;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.IRepositories;
using EnglishCenter.Presentation.Helpers;
using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;
using Microsoft.AspNetCore.Identity;

namespace EnglishCenter.DataAccess.Repositories.AuthenticationRepositories
{
    public class TeacherRepository : GenericRepository<Teacher>, ITeacherRepository
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<User> _userManager;

        public TeacherRepository(EnglishCenterContext context, IWebHostEnvironment webHostEnvironment, UserManager<User> userManager) : base(context)
        {
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
        }

        public async Task<bool> ChangeBackgroundImageAsync(IFormFile file, Teacher teacher)
        {
            if (teacher == null) return false;

            var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "users", "backgrounds");
            var fileName = $"{DateTime.Now.Ticks}_{file.FileName}";
            var result = await UploadHelper.UploadFileAsync(file, uploadFolder, fileName);

            if (!string.IsNullOrEmpty(result)) return false;

            var wwwRootPath = _webHostEnvironment.WebRootPath;
            var previousImage = Path.Combine(wwwRootPath, teacher.BackgroundImage ?? "");

            if (File.Exists(previousImage))
            {
                File.Delete(previousImage);
            }

            teacher.BackgroundImage = Path.Combine("users", "backgrounds", fileName);
            return true;
        }

        public async Task<bool> ChangeTeacherImageAsync(IFormFile file, Teacher teacher)
        {
            if (teacher == null) return false;

            var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "users", "profiles");
            var fileName = $"{DateTime.Now.Ticks}_{file.FileName}";
            var result = await UploadHelper.UploadFileAsync(file, uploadFolder, fileName);

            if (!string.IsNullOrEmpty(result)) return false;

            var wwwRootPath = _webHostEnvironment.WebRootPath;
            var previousImage = Path.Combine(wwwRootPath, teacher.Image ?? "");

            if (File.Exists(previousImage))
            {
                File.Delete(previousImage);
            }

            teacher.Image = Path.Combine("users", "profiles", fileName);
            return true;
        }

        public async Task<Response> ChangePasswordAsync(Teacher teacher, string currentPassword, string newPassword)
        {
            if (teacher == null || string.IsNullOrEmpty(currentPassword) || string.IsNullOrEmpty(newPassword))
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "You need to enter all information",
                    Success = false
                };
            }

            if (currentPassword == newPassword)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "You should set a new password different from your current password",
                    Success = false
                };
            }

            var user = await _userManager.FindByIdAsync(teacher.UserId);

            if (user == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any users",
                    Success = false
                };
            };

            var resetResult = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);

            if (!resetResult.Succeeded)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = resetResult.Errors.Select(e => e.Description).ToList(),
                    Success = false
                };
            }

            return new Response()
            {
                Message = "",
                StatusCode = System.Net.HttpStatusCode.OK,
                Success = true
            };
        }

        public async Task<Response> ChangeTeacherInfoASync(Teacher teacher, UserInfoDto model)
        {
            var user = await _userManager.FindByIdAsync(teacher.UserId);
            if (teacher == null || user == null)
            {
                return new Response()
                {
                    Message = "Can't find any teachers",
                    Success = false,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };
            }

            teacher.FirstName = model.FirstName;
            teacher.LastName = model.LastName;
            teacher.Gender = model.Gender;
            teacher.DateOfBirth = model.DateOfBirth;
            teacher.PhoneNumber = model.PhoneNumber;
            teacher.Address = model.Address;

            if (!string.IsNullOrEmpty(model.Email) && user.Email != model.Email)
            {
                var emailToken = await _userManager.GenerateChangeEmailTokenAsync(user, model.Email);
                var changeResult = await _userManager.ChangeEmailAsync(user, model.Email, emailToken);

                if (!changeResult.Succeeded)
                {
                    return new Response()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Message = changeResult.Errors.Select(e => e.Description).ToList(),
                        Success = false
                    };
                }
            }

            if (!string.IsNullOrEmpty(model.PhoneNumber) && user.PhoneNumber != model.PhoneNumber)
            {
                var phoneToken = await _userManager.GenerateChangePhoneNumberTokenAsync(user, model.PhoneNumber);
                var changeResult = await _userManager.ChangePhoneNumberAsync(user, model.PhoneNumber, phoneToken);

                if (!changeResult.Succeeded)
                {
                    return new Response()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Message = changeResult.Errors.Select(e => e.Description).ToList(),
                        Success = false
                    };
                }
            }

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "",
                Success = true
            };
        }

        public string GetFullName(Teacher teacherModel)
        {
            if (teacherModel == null) return string.Empty;

            var result = teacherModel.FirstName + " " + teacherModel.LastName;
            return result.Trim();
        }

        public async Task<string> GetFullNameAsync(string userId)
        {
            var teacherModel = await context.Teachers.FindAsync(userId);
            if (teacherModel == null) return string.Empty;

            string result = teacherModel.FirstName + " " + teacherModel.LastName;
            return result.Trim();
        }

        public Task<bool> ChangeTeacherBackgroundAsync(Teacher teacher, UserBackgroundDto stuModel)
        {
            if (teacher == null) return Task.FromResult(false);

            teacher.UserName = stuModel.UserName;
            teacher.Description = stuModel.Description;

            return Task.FromResult(true);
        }
    }
}
