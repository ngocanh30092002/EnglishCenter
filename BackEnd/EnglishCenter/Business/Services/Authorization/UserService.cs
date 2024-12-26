using System.Data;
using AutoMapper;
using EnglishCenter.Business.IServices;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.UnitOfWork;
using EnglishCenter.Presentation.Global;
using EnglishCenter.Presentation.Global.Enum;
using EnglishCenter.Presentation.Helpers;
using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;
using EnglishCenter.Presentation.Models.ResDTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.Business.Services.Authorization
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unit;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ISubmissionFileService _submissionFileService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UserService(
            UserManager<User> userManager
            , IMapper mapper
            , RoleManager<IdentityRole> roleManager
            , IUnitOfWork unit
            , IWebHostEnvironment webHostEnvironment
            , ISubmissionFileService submissionFileService)
        {
            _unit = unit;
            _userManager = userManager;
            _mapper = mapper;
            _roleManager = roleManager;
            _submissionFileService = submissionFileService;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<Response> GetAllAsync(string userId)
        {
            var userModels = await _userManager.Users
                                    .Include(u => u.Student)
                                    .Include(u => u.Teacher)
                                    .Where(u => u.Id != userId)
                                    .ToListAsync();

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<UserResDto>>(userModels),
                Success = true
            };
        }

        public async Task<Response> GetUserBackgroundInfoAsync(string userId)
        {
            var userModel = await _userManager.Users
                                              .Include(u => u.Teacher)
                                              .Include(u => u.Student)
                                              .FirstOrDefaultAsync(u => u.Id == userId);
            if (userModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "",
                    Success = false
                };
            }

            var userBgDto = new UserBackgroundDto();
            var rolesOfUser = await _userManager.GetRolesAsync(userModel);
            if (rolesOfUser.Any(r => r == AppRole.TEACHER))
            {
                userBgDto = _mapper.Map<UserBackgroundDto>(userModel.Teacher);
                userBgDto.Roles = rolesOfUser.ToList();


                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Message = userBgDto,
                    Success = true
                };
            }

            userBgDto = _mapper.Map<UserBackgroundDto>(userModel.Student);
            userBgDto.Roles = rolesOfUser.ToList();

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = userBgDto,
                Success = true
            };
        }

        public async Task<Response> UpdateAsync(UserDto model)
        {
            var userModel = await _userManager.Users
                                    .Include(u => u.Student)
                                    .Include(u => u.Teacher)
                                    .FirstOrDefaultAsync(u => u.Id == model.UserId);
            if (userModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any users",
                    Success = false
                };
            }

            if (userModel.UserName != model.UserName)
            {
                userModel.UserName = model.UserName;
            }
            if (userModel.Email != model.UserEmail)
            {
                userModel.Email = model.UserEmail;
                userModel.NormalizedEmail = model.UserEmail.ToUpper();
            }

            if (userModel.Student != null)
            {
                userModel.Student.Address = model.Address;
                userModel.Student.PhoneNumber = model.PhoneNumber;

                if (string.IsNullOrEmpty(model.DateOfBirth))
                {
                    userModel.Student.DateOfBirth = null;
                }
                else
                {
                    if (!DateOnly.TryParse(model.DateOfBirth, out var dateOnly))
                    {
                        return new Response()
                        {
                            StatusCode = System.Net.HttpStatusCode.BadRequest,
                            Message = "Date of birth is invalid",
                            Success = false
                        };
                    }

                    userModel.Student.DateOfBirth = dateOnly;
                }
            }

            if (userModel.Teacher != null)
            {
                userModel.Teacher.Address = model.Address;
                userModel.Teacher.PhoneNumber = model.PhoneNumber;

                if (string.IsNullOrEmpty(model.DateOfBirth))
                {
                    userModel.Teacher.DateOfBirth = null;
                }
                else
                {
                    if (!DateOnly.TryParse(model.DateOfBirth, out var dateOnly))
                    {
                        return new Response()
                        {
                            StatusCode = System.Net.HttpStatusCode.BadRequest,
                            Message = "Date of birth is invalid",
                            Success = false
                        };
                    }

                    userModel.Teacher.DateOfBirth = dateOnly;
                }
            }


            userModel.EmailConfirmed = true;

            if (model.Locked == 0)
            {
                userModel.LockoutEnd = null;
            }
            else
            {
                userModel.LockoutEnd = DateTime.Now.AddHours(5);
            }

            await _userManager.UpdateAsync(userModel);

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "",
                Success = true
            };
        }

        public async Task<Response> GetUserInfoAsync(string userId)
        {
            var userModel = await _userManager.Users
                                              .Include(u => u.Teacher)
                                              .Include(u => u.Student)
                                              .FirstOrDefaultAsync(u => u.Id == userId);
            if (userModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "",
                    Success = false
                };
            }

            var userInfoDto = new UserInfoDto();
            var rolesOfUser = await _userManager.GetRolesAsync(userModel);
            if (rolesOfUser.Any(r => r == AppRole.TEACHER))
            {
                userInfoDto = _mapper.Map<UserInfoDto>(userModel.Teacher);
                userInfoDto.Email = userModel.Email ?? "";


                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Message = userInfoDto,
                    Success = true
                };
            }

            userInfoDto = _mapper.Map<UserInfoDto>(userModel.Student);
            userInfoDto.Email = userModel.Email ?? "";

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = userInfoDto,
                Success = true
            };

        }

        public async Task<Response> GetUserFullInfoAsync(string userId)
        {
            var userBgRes = await GetUserBackgroundInfoAsync(userId);
            var userInfoRes = await GetUserInfoAsync(userId);

            var result = new UserInfoResDto();

            if (!userBgRes.Success) return userBgRes;
            if (!userInfoRes.Success) return userInfoRes;
            var studentBg = userBgRes.Message as UserBackgroundDto;
            var studentInfo = userInfoRes.Message as UserInfoDto;

            result.FirstName = studentInfo!.FirstName;
            result.LastName = studentInfo!.LastName;
            result.Gender = studentInfo!.Gender;
            result.DateOfBirth = studentInfo!.DateOfBirth;
            result.PhoneNumber = studentInfo!.PhoneNumber;
            result.Email = studentInfo!.Email;
            result.Address = studentInfo!.Address;
            result.UserName = studentInfo!.UserName;
            result.Description = studentBg!.Description;
            result.Roles = studentBg!.Roles;
            result.Image = studentBg!.Image;
            result.BackgroundImage = studentBg!.BackgroundImage;


            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = result,
                Success = true
            };
        }

        public async Task<Response> ChangeUserInfoAsync(string userId, UserInfoDto model)
        {
            var userModel = await _userManager.Users
                                               .Include(u => u.Teacher)
                                               .Include(u => u.Student)
                                               .FirstOrDefaultAsync(u => u.Id == userId);
            if (userModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any records",
                    Success = false
                };
            }

            var rolesOfUser = await _userManager.GetRolesAsync(userModel);
            if (rolesOfUser.Any(r => r == AppRole.TEACHER))
            {
                var changeRes = await _unit.Teachers.ChangeTeacherInfoASync(userModel.Teacher, model);
                if (!changeRes.Success) return changeRes;

            }

            var response = await _unit.Students.ChangeStudentInfoAsync(userModel.Student, model);

            if (response.Success)
            {
                await _unit.CompleteAsync();
            }

            return response;
        }

        public async Task<Response> DeleteAsync(string userId)
        {
            var userModel = await _userManager.FindByIdAsync(userId);
            if (userModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any users",
                    Success = false
                };
            }

            var teacherModel = _unit.Teachers.GetById(userId);
            if (teacherModel != null)
            {
                var previousImage = Path.Combine(_webHostEnvironment.WebRootPath, teacherModel.Image ?? "");
                var previousBgImage = Path.Combine(_webHostEnvironment.WebRootPath, teacherModel.BackgroundImage ?? "");

                if (File.Exists(previousImage))
                {
                    File.Delete(previousImage);
                }

                if (File.Exists(previousBgImage))
                {
                    File.Delete(previousBgImage);
                }
            }

            var studentModel = _unit.Students.GetById(userId);
            if (studentModel != null)
            {
                var previousImage = Path.Combine(_webHostEnvironment.WebRootPath, studentModel.Image ?? "");
                var previousBgImage = Path.Combine(_webHostEnvironment.WebRootPath, studentModel.BackgroundImage ?? "");

                if (File.Exists(previousImage))
                {
                    File.Delete(previousImage);
                }

                if (File.Exists(previousBgImage))
                {
                    File.Delete(previousBgImage);
                }
            }


            var enrollIds = _unit.Enrollment
                                   .Find(e => e.UserId == userModel.Id)
                                   .Select(e => e.EnrollId)
                                   .ToList();

            foreach (var enrollId in enrollIds)
            {
                var deleteRes = await DeleteEnrollAsync(enrollId);
                if (!deleteRes.Success) return deleteRes;
            }

            var deleteSuccess = await _userManager.DeleteAsync(userModel);

            if (deleteSuccess.Succeeded)
            {

                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Message = "",
                    Success = true
                };
            }

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.BadRequest,
                Message = "Can't delete user",
                Success = false
            };
        }

        public async Task<Response> ChangeUserBackgroundAsync(string userId, UserBackgroundDto stuModel)
        {
            var userModel = await _userManager.Users
                                               .Include(u => u.Teacher)
                                               .Include(u => u.Student)
                                               .FirstOrDefaultAsync(u => u.Id == userId);
            if (userModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any records",
                    Success = false
                };
            }

            var rolesOfUser = await _userManager.GetRolesAsync(userModel);
            if (rolesOfUser.Any(r => r == AppRole.TEACHER))
            {
                var isSuccess = await _unit.Teachers.ChangeTeacherBackgroundAsync(userModel.Teacher, stuModel);
                if (!isSuccess)
                {
                    return new Response()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Message = "Change teacher background failed",
                        Success = false
                    };
                };

            }

            var isSuccessStu = await _unit.Students.ChangeStudentBackgroundAsync(userModel.Student, stuModel);
            if (!isSuccessStu)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Change student background failed",
                    Success = false
                };
            }

            await _unit.CompleteAsync();

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "",
                Success = true
            };
        }

        public async Task<Response> ChangePasswordAsync(string userId, string currentPassword, string newPassword)
        {
            var userModel = await _userManager.FindByIdAsync(userId);
            if (userModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any users",
                    Success = false
                };
            }

            if (string.IsNullOrEmpty(currentPassword) || string.IsNullOrEmpty(newPassword))
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

            var resetResult = await _userManager.ChangePasswordAsync(userModel, currentPassword, newPassword);

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

        public async Task<Response> ChangeUserImageAsync(string userId, IFormFile file)
        {
            var userModel = await _userManager.Users
                                              .Include(u => u.Teacher)
                                              .Include(u => u.Student)
                                              .FirstOrDefaultAsync(u => u.Id == userId);
            if (userModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any users",
                    Success = false
                };
            }

            var rolesOfUser = await _userManager.GetRolesAsync(userModel);
            var previousFile = Path.Combine(_webHostEnvironment.WebRootPath, userModel.Student?.Image ?? "");
            if (File.Exists(previousFile))
            {
                File.Delete(previousFile);
            }

            var uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "users", "profiles");
            var fileName = $"{DateTime.Now.Ticks}_{file.FileName}";
            var result = await UploadHelper.UploadFileAsync(file, uploadFolder, fileName);
            if (!string.IsNullOrEmpty(result))
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = result,
                    Success = false
                };
            }

            if (rolesOfUser.Any(r => r == AppRole.TEACHER))
            {
                var teacherModel = _unit.Teachers.GetById(userModel.Id);
                teacherModel.Image = Path.Combine("users", "profiles", fileName);
            }

            var studentModel = _unit.Students.GetById(userModel.Id);
            studentModel.Image = Path.Combine("users", "profiles", fileName);

            await _unit.CompleteAsync();
            await _userManager.UpdateAsync(userModel);

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "",
                Success = true
            };
        }

        public async Task<Response> ChangeUserBackgroundImageAsync(string userId, IFormFile file)
        {
            var userModel = await _userManager.Users
                                              .Include(u => u.Teacher)
                                              .Include(u => u.Student)
                                              .FirstOrDefaultAsync(u => u.Id == userId);
            if (userModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any users",
                    Success = false
                };
            }

            var rolesOfUser = await _userManager.GetRolesAsync(userModel);
            var previousFile = Path.Combine(_webHostEnvironment.WebRootPath, userModel.Student?.BackgroundImage ?? "");
            if (File.Exists(previousFile))
            {
                File.Delete(previousFile);
            }

            var uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "users", "backgrounds");
            var fileName = $"{DateTime.Now.Ticks}_{file.FileName}";
            var result = await UploadHelper.UploadFileAsync(file, uploadFolder, fileName);
            if (!string.IsNullOrEmpty(result))
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = result,
                    Success = false
                };
            }

            if (rolesOfUser.Any(r => r == AppRole.TEACHER))
            {
                var teacherModel = _unit.Teachers.GetById(userModel.Id);
                teacherModel.BackgroundImage = Path.Combine("users", "backgrounds", fileName);
            }

            var studentModel = _unit.Students.GetById(userModel.Id);
            studentModel.BackgroundImage = Path.Combine("users", "backgrounds", fileName);

            await _unit.CompleteAsync();
            await _userManager.UpdateAsync(userModel);

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "",
                Success = true
            };
        }

        public async Task<Response> GetUsersWithRolesAsync()
        {
            var userModels = await _userManager.Users
                                .Include(u => u.Teacher)
                                .Include(u => u.Student)
                                .ToListAsync();

            var userResDtos = new List<UserResDto>();

            foreach (var user in userModels)
            {
                var userResDto = _mapper.Map<UserResDto>(user);
                userResDto.Roles = (await _userManager.GetRolesAsync(user)).ToList();

                userResDtos.Add(userResDto);
            }

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = userResDtos,
                Success = true
            };
        }

        public async Task<Response> ChangePasswordAsync(string userId, string newPassword)
        {
            var userModel = await _userManager.FindByIdAsync(userId);
            if (userModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any users",
                    Success = false
                };
            }

            var removePassword = await _userManager.RemovePasswordAsync(userModel);
            if (!removePassword.Succeeded)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't change password",
                    Success = false
                };
            }

            var addPassword = await _userManager.AddPasswordAsync(userModel, newPassword);
            if (!addPassword.Succeeded)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't change password",
                    Success = false
                };
            }

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "",
                Success = true
            };
        }

        private async Task<Response> DeleteEnrollAsync(long enrollmentId)
        {
            var enrollmentModel = _unit.Enrollment.GetById(enrollmentId);

            if (enrollmentModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any enrollments"
                };
            }

            if (enrollmentModel.ScoreHisId.HasValue)
            {
                var scoreHisModel = _unit.ScoreHis.GetById(enrollmentModel.ScoreHisId.Value);

                _unit.ScoreHis.Remove(scoreHisModel);
            }

            var classModel = _unit.Classes.GetById(enrollmentModel.ClassId);

            if (enrollmentModel.StatusId == (int)EnrollEnum.Pending)
            {
                classModel.RegisteringNum--;
            }
            else if (enrollmentModel.StatusId != (int)EnrollEnum.Rejected)
            {
                classModel.RegisteredNum--;
            }

            var attendances = _unit.Attendances.Find(s => s.EnrollId == enrollmentId).ToList();
            var hwSubmission = _unit.HwSubmissions.Find(s => s.EnrollId == enrollmentId).ToList();

            _unit.Attendances.RemoveRange(attendances);
            _unit.HwSubmissions.RemoveRange(hwSubmission);

            var submissionFileIds = _unit.SubmissionFiles
                                         .Find(s => s.EnrollId == enrollmentId)
                                         .Select(s => s.SubmissionFileId)
                                         .ToList();

            foreach (var submissionId in submissionFileIds)
            {
                var deleteRes = await _submissionFileService.DeleteAsync(submissionId);
                if (!deleteRes.Success) return deleteRes;
            }

            _unit.Enrollment.Remove(enrollmentModel);


            await _unit.CompleteAsync();

            await _unit.Notifications.SendNotificationToUser(enrollmentModel.UserId, classModel.ClassId, new NotiDto()
            {
                Title = "Class message",
                Description = $"You have been removed from class {classModel.ClassId} by the teacher or admin",
                Image = classModel.Image,
                IsRead = false,
                Time = DateTime.Now,
            });

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "",
                Success = true
            };
        }
    }
}
