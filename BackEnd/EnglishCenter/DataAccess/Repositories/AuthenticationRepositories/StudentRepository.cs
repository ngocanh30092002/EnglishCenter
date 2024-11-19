using AutoMapper;
using EnglishCenter.DataAccess.Database;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.IRepositories;
using EnglishCenter.Presentation.Helpers;
using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;
using Microsoft.AspNetCore.Identity;

namespace EnglishCenter.DataAccess.Repositories.AuthenticationRepositories
{
    public class StudentRepository : GenericRepository<Student>, IStudentRepository
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public StudentRepository(
            EnglishCenterContext context,
            IWebHostEnvironment webHostEnvironment,
            UserManager<User> userManager,
            IMapper mapper
            ) : base(context)
        {
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<bool> ChangeStudentImageAsync(IFormFile file, Student student)
        {
            if (student == null) return false;

            var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "users", "profiles");
            var fileName = $"{DateTime.Now.Ticks}_{file.FileName}";
            var result = await UploadHelper.UploadFileAsync(file, uploadFolder, fileName);

            if (!string.IsNullOrEmpty(result)) return false;

            var wwwRootPath = _webHostEnvironment.WebRootPath;
            var previousImage = Path.Combine(wwwRootPath, student.Image ?? "");

            if (File.Exists(previousImage))
            {
                File.Delete(previousImage);
            }

            student.Image = Path.Combine("users", "profiles", fileName);
            return true;
        }

        public async Task<bool> ChangeBackgroundImageAsync(IFormFile file, Student student)
        {
            if (student == null) return false;

            var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "users", "backgrounds");
            var fileName = $"{DateTime.Now.Ticks}_{file.FileName}";
            var result = await UploadHelper.UploadFileAsync(file, uploadFolder, fileName);

            if (!string.IsNullOrEmpty(result)) return false;

            var wwwRootPath = _webHostEnvironment.WebRootPath;
            var previousImage = Path.Combine(wwwRootPath, student.BackgroundImage ?? "");

            if (File.Exists(previousImage))
            {
                File.Delete(previousImage);
            }

            student.BackgroundImage = Path.Combine("users", "backgrounds", fileName);
            return true;
        }

        public async Task<Response> ChangePasswordAsync(Student student, string currentPassword, string newPassword)
        {
            if (student == null || string.IsNullOrEmpty(currentPassword) || string.IsNullOrEmpty(newPassword))
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

            var user = await _userManager.FindByIdAsync(student.UserId);

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

        public async Task<Response> ChangeStudentInfoAsync(Student student, StudentInfoDto model)
        {
            var user = await _userManager.FindByIdAsync(student.UserId);
            if (student == null || user == null)
            {
                return new Response()
                {
                    Message = "Can't find any students",
                    Success = false,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };
            }

            student.FirstName = model.FirstName;
            student.LastName = model.LastName;
            student.Gender = model.Gender;
            student.DateOfBirth = model.DateOfBirth;
            student.PhoneNumber = model.PhoneNumber;
            student.Address = model.Address;

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

        public bool ChangeStudentBackground(Student student, StudentBackgroundDto stuModel)
        {
            if (student == null) return false;

            student.UserName = stuModel.UserName;
            student.Description = stuModel.Description;

            return true;
        }

        public async Task<StudentInfoDto?> GetStudentInfoAsync(Student student)
        {
            var user = await _userManager.FindByIdAsync(student.UserId);

            if (student == null || user == null)
            {
                return null;
            }

            var userDto = _mapper.Map<StudentInfoDto>(student);
            userDto.Email = user.Email ?? "";

            return userDto;
        }

        public async Task<StudentBackgroundDto?> GetStudentBackgroundAsync(Student student)
        {
            var user = await _userManager.FindByIdAsync(student.UserId);

            if (student == null || user == null)
            {
                return null;
            }

            var roleOfUser = await _userManager.GetRolesAsync(user);

            var userBackgroundDto = _mapper.Map<StudentBackgroundDto>(student);
            userBackgroundDto.Roles = roleOfUser.ToList();

            return userBackgroundDto;
        }
    }
}
