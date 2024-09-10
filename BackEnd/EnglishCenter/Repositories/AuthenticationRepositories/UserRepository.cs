using AutoMapper;
using EnglishCenter.Database;
using EnglishCenter.Helpers;
using EnglishCenter.Models;
using EnglishCenter.Models.DTO;
using EnglishCenter.Repositories.IRepositories;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace EnglishCenter.Repositories.AuthenticationRepositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly EnglishCenterContext _context;
        private readonly IMapper _mapper;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UserRepository(
            EnglishCenterContext context,
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            IMapper mapper,
            IWebHostEnvironment webHostEnvironment) 
        {
            _userManager = userManager;
            _context = context;
            _mapper = mapper;
            _roleManager = roleManager;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<Response> ChangeUserImageAsync(IFormFile file, string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return new Response()
                {
                    Message = "User not found",
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Success = false
                };
            }

            var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "users", "profiles");
            var fileName = $"{DateTime.Now.Ticks}_{file.FileName}";
            var result = await UploadHelper.UploadFileAsync(file, uploadFolder, fileName);

            if (!string.IsNullOrEmpty(result))
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Success = false,
                    Message = result
                };
            }

            var student = await _context.Students.FindAsync(userId);
            if (student == null)
            {
                return new Response()
                {
                    Message = "Can't find any students",
                    Success = false,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };
            }

            var wwwRootPath = _webHostEnvironment.WebRootPath;
            var previousImage = Path.Combine(wwwRootPath, student.Image);

            if (File.Exists(previousImage))
            {
                File.Delete(previousImage);
            }

            student.Image = Path.Combine("users", "profiles", fileName);
          
            await _context.SaveChangesAsync();

            return new Response()
            {
                Success = true,
                StatusCode = System.Net.HttpStatusCode.OK,
            };
        }

        public async Task<Response> ChangeBackgroundImageAsync(IFormFile file, string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return new Response()
                {
                    Message = "User not found",
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Success = false
                };
            }

            var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "users", "backgrounds");
            var fileName = $"{DateTime.Now.Ticks}_{file.FileName}";
            var result = await UploadHelper.UploadFileAsync(file, uploadFolder, fileName);

            if (!string.IsNullOrEmpty(result))
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Success = false,
                    Message = result
                };
            }

            var student = await _context.Students.FindAsync(userId);
            if (student == null)
            {
                return new Response()
                {
                    Message = "Can't find any students",
                    Success = false,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };
            }

            var wwwRootPath = _webHostEnvironment.WebRootPath;
            var previousImage = Path.Combine(wwwRootPath, student.BackgroundImage);

            if (File.Exists(previousImage))
            {
                File.Delete(previousImage);
            }

            student.BackgroundImage = Path.Combine("users", "backgrounds", fileName);

            await _context.SaveChangesAsync();

            return new Response()
            {
                Success = true,
                StatusCode = System.Net.HttpStatusCode.OK,
            };
        }

        public async Task<Response> ChangePasswordAsync(string userId,string currentPassword, string newPassword)
        {
             
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(currentPassword) || string.IsNullOrEmpty(newPassword))
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "You need to enter all information",
                    Success = false
                };
            }

            if(currentPassword == newPassword)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "You should set a new password different from your current password",
                    Success = false
                };
            }

            var user = await _userManager.FindByIdAsync(userId);

            if(user == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any users",
                    Success = false
                };
            }

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

        public async Task<Response> ChangeUserInfoAsync(string userId, UserInfoDtoModel model)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return new Response()
                {
                    Message = "User not found",
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Success = false
                };
            }

            var student = await _context.Students.FindAsync(userId);
            var user = await _userManager.FindByIdAsync(userId);

            if(student == null || user == null)
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

            if(!string.IsNullOrEmpty(model.PhoneNumber) && user.PhoneNumber != model.PhoneNumber)
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

            await _context.SaveChangesAsync();
            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "",
                Success = true
            };
        }

        public async Task<Response> ChangeUserBackgroundAsync(string userId, UserBackgroundDtoModel stuModel)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return new Response()
                {
                    Message = "User not found",
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Success = false
                };
            }

            var student = await _context.Students.FindAsync(userId);

            if (student == null)
            {
                return new Response()
                {
                    Message = "Can't find any students",
                    Success = false,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };
            }

            student.UserName = stuModel.UserName;
            student.Description = stuModel.Description;

            await _context.SaveChangesAsync();

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "",
                Success = true
            };
        }

        public async Task<Response> GetUserInfo(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return new Response()
                {
                    Message = "User not found",
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Success = false
                };
            }

            var student = await _context.Students.FindAsync(userId);
            var user = await _userManager.FindByIdAsync(userId);

            if (student == null || user == null)
            {
                return new Response()
                {
                    Message = "Can't find any students",
                    Success = false,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };
            }

            var userDto = _mapper.Map<UserInfoDtoModel>(student);
            userDto.Email = user.Email;


            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Success = true,
                Message = userDto
            };
        }

        public async Task<Response> GetUserBackground(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return new Response()
                {
                    Message = "User not found",
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Success = false
                };
            }

            var student = await _context.Students.FindAsync(userId);
            var user = await _userManager.FindByIdAsync(userId);

            if (student == null || user == null )
            {
                return new Response()
                {
                    Message = "Can't find any students",
                    Success = false,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };
            }

            var roleOfUser = await _userManager.GetRolesAsync(user);

            var userBackgroundDto = _mapper.Map<UserBackgroundDtoModel>(student);
            userBackgroundDto.Roles = roleOfUser.ToList();

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Success = true,
                Message = userBackgroundDto
            };
        }
    }
}
