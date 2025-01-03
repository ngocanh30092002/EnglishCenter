﻿using AutoMapper;
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

namespace EnglishCenter.Business.Services.Courses
{
    public class ClassService : IClassService
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;
        private readonly IClaimService _claimService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<User> _userManager;
        private string _imageBase;
        private readonly IEnrollmentService _enrollService;
        private readonly IClassScheduleService _classScheduleService;
        private readonly IClassMaterialService _classMaterialService;

        public ClassService(
            IMapper mapper,
            IUnitOfWork unit,
            IWebHostEnvironment webHostEnvironment,
            IClaimService claimService,
            UserManager<User> userManager,
            IEnrollmentService enrollService,
            IClassScheduleService classScheduleService,
            IClassMaterialService classMaterialService
            )
        {
            _unit = unit;
            _mapper = mapper;
            _claimService = claimService;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
            _imageBase = Path.Combine("classes", "images");
            _enrollService = enrollService;
            _classScheduleService = classScheduleService;
            _classMaterialService = classMaterialService;
        }

        public async Task<Response> ChangeDescriptionAsync(string classId, string newDes)
        {
            var classModel = _unit.Classes.GetById(classId);
            if (classModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any classes",
                    Success = false
                };
            }

            var isSuccess = await _unit.Classes.ChangeDescriptionAsync(classModel, newDes);
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
                Message = "",
                Success = true
            };
        }

        public async Task<Response> ChangeCourseAsync(string classId, string courseId)
        {
            var isExistCourse = _unit.Courses.IsExist(c => c.CourseId == courseId);
            if (!isExistCourse)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any courses",
                    Success = false
                };
            }

            var classModel = _unit.Classes.GetById(classId);
            if (classModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Success = false,
                    Message = "Can't find any classes"
                };
            }

            var isSuccess = await _unit.Classes.ChangeCourseAsync(classModel, courseId);
            if (!isSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Success = false,
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

        public async Task<Response> ChangeEndTimeAsync(string classId, DateOnly endTime)
        {
            var classModel = _unit.Classes.GetById(classId);

            if (classModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Success = false,
                    Message = "Can't find any classes"
                };
            }

            if (classModel.StartDate.HasValue)
            {
                if (classModel.StartDate.Value > endTime)
                {
                    return new Response()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Success = false,
                        Message = "EndDate must be greater than StartDate"
                    };
                }
            }

            var isSuccess = await _unit.Classes.ChangeEndTimeAsync(classModel, endTime);
            if (!isSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Success = false,
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

        public async Task<Response> ChangeMaxNumAsync(string classId, int maxNum)
        {
            var classModel = _unit.Classes.GetById(classId);
            if (classModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Success = false,
                    Message = "Can't find any classes"
                };
            }

            if (maxNum < 0)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Success = false,
                    Message = "MaxNum must be greater than 0"
                };
            }

            var isSuccess = await _unit.Classes.ChangeMaxNumAsync(classModel, maxNum);

            if (!isSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Success = false,
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

        public async Task<Response> ChangeImageAsync(string classId, IFormFile image)
        {
            var classModel = _unit.Classes.GetById(classId);
            if (classModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Success = false,
                    Message = "Can't find any classes"
                };
            }

            var isSuccess = await _unit.Classes.ChangeImageAsync(classModel, image);

            if (!isSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Success = false,
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

        public async Task<Response> ChangeStartTimeAsync(string classId, DateOnly startTime)
        {
            var classModel = _unit.Classes.GetById(classId);

            if (classModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Success = false,
                    Message = "Can't find any classes"
                };
            }

            if (classModel.EndDate.HasValue)
            {
                if (classModel.EndDate.Value < startTime)
                {
                    return new Response()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Success = false,
                        Message = "StartDate must be less than EndDate"
                    };
                }
            }

            var isSuccess = await _unit.Classes.ChangeStartTimeAsync(classModel, startTime);
            if (!isSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Success = false,
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

        public async Task<Response> CreateAsync(ClassDto model)
        {
            var isExistCourse = _unit.Courses.IsExist(c => c.CourseId == model.CourseId);
            if (!isExistCourse)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any courses",
                    Success = false
                };
            }

            var isExistClass = _unit.Classes.IsExist(c => c.ClassId == model.ClassId);
            if (isExistClass)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Class already exists",
                };
            }

            var isExistTeacher = _unit.Teachers.IsExist(s => s.UserId == model.TeacherId);
            if (!isExistTeacher)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any teachers"
                };
            }

            if (!model.StartDate.HasValue || !model.EndDate.HasValue)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Start Date , End Date is required",
                    Success = false
                };
            }

            if (model.StartDate.HasValue && model.EndDate.HasValue)
            {
                if (model.StartDate.Value > model.EndDate.Value)
                {
                    return new Response()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Message = "Start time must be less than end time"
                    };
                }
            }

            var classModel = _mapper.Map<Class>(model);
            if (model.Image != null)
            {
                var uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, _imageBase);
                var fileName = $"{DateTime.Now.Ticks}_{model.Image.FileName}";
                var result = await UploadHelper.UploadFileAsync(model.Image, uploadFolder, fileName);

                if (!string.IsNullOrEmpty(result))
                {
                    return new Response()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Message = result
                    };
                }

                classModel.Image = Path.Combine(_imageBase, fileName);
            }

            var currentDate = DateOnly.FromDateTime(DateTime.Now);

            if (model.StartDate.Value >= currentDate)
            {
                classModel.Status = (int)ClassEnum.Waiting;
            }
            if (model.StartDate.Value <= currentDate && currentDate <= model.EndDate.Value)
            {
                classModel.Status = (int)ClassEnum.Opening;
            }

            if (model.EndDate.Value <= currentDate)
            {
                classModel.Status = (int)ClassEnum.End;
            }

            _unit.Classes.Add(classModel);

            var isAddClaimSuccess = await _claimService.AddClaimToUserAsync(model.TeacherId, new ClaimDto()
            {
                ClaimName = GlobalClaimNames.CLASS,
                ClaimValue = model.ClassId
            });

            if (!isAddClaimSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't add claim to user",
                    Success = false
                };
            }

            await _unit.Notifications.CreateGroupAsync(classModel.ClassId);
            await _unit.CompleteAsync();
            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "",
                Success = true
            };
        }

        public async Task<Response> DeleteAsync(string classId)
        {
            var classModel = await _unit.Classes
                                        .Include(c => c.Enrollments)
                                        .FirstOrDefaultAsync(c => c.ClassId == classId);
            if (classModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any classes",
                    Success = false
                };
            }

            var isValidStatus = classModel.Status != (int)ClassEnum.Opening;
            if (!isValidStatus)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Class is opening so can't delete it",
                    Success = false
                };
            }

            if (!string.IsNullOrEmpty(classModel.Image))
            {
                var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, _imageBase);
                if (File.Exists(imagePath))
                {
                    File.Delete(imagePath);
                }
            }

            var isAnyStudentOngoing = classModel.Enrollments.Any(e => e.StatusId == (int)EnrollEnum.Ongoing);
            if (isAnyStudentOngoing)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "There are still students currently studying so the class can't be deleted.",
                    Success = false
                };
            }

            await _unit.BeginTransAsync();

            try
            {
                var enrollIds = classModel.Enrollments.Select(e => e.EnrollId).ToList();


                foreach (var enrollId in enrollIds)
                {
                    var res = await _enrollService.DeleteAsync(enrollId);
                    if (!res.Success) return res;
                }

                var classMaterialIds = _unit.ClassMaterials
                                       .Find(e => e.ClassId == classModel.ClassId)
                                       .Select(s => s.ClassMaterialId)
                                       .ToList();

                foreach (var id in classMaterialIds)
                {
                    var res = await _classMaterialService.DeleteAsync(id);
                    if (!res.Success) return res;
                }


                var classScheduleIds = _unit.ClassSchedules
                                            .Find(e => e.ClassId == classModel.ClassId)
                                            .Select(s => s.ScheduleId)
                                            .ToList();

                foreach (var id in classScheduleIds)
                {
                    var res = await _classScheduleService.DeleteAsync(id);
                    if (!res.Success) return res;
                }

                _unit.Classes.Remove(classModel);

                var isDeleteSuccess = await _claimService.DeleteClaimInUserAsync(classModel.TeacherId, new ClaimDto()
                {
                    ClaimName = GlobalClaimNames.CLASS,
                    ClaimValue = classModel.ClassId
                });

                if (!isDeleteSuccess)
                {
                    return new Response()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Message = "Can't delete claim of user",
                        Success = false
                    };
                }





                await _unit.CompleteAsync();
                await _unit.CommitTransAsync();

                return new Response()
                {
                    Success = true,
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Message = ""
                };
            }
            catch (Exception ex)
            {
                await _unit.RollBackTransAsync();
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = ex.Message,
                    Success = false
                };
            }
        }

        public Task<Response> GetAllAsync()
        {
            var classes = _unit.Classes.GetAll().ToList();

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<ClassResDto>>(classes),
                Success = true
            });
        }

        public async Task<Response> GetClassesWithCourseAsync(string courseId)
        {
            var isExistCourse = _unit.Courses.IsExist(c => c.CourseId == courseId);
            if (!isExistCourse)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any courses",
                    Success = false
                };
            }

            var currentDate = DateOnly.FromDateTime(DateTime.Now);

            var classes = await _unit.Classes.Include(c => c.Teacher)
                                       .Where(c => c.CourseId == courseId &&
                                                   c.Status != (int)ClassEnum.End &&
                                                   c.StartDate <= currentDate && currentDate <= c.EndDate)
                                       .ToListAsync();

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<ClassResDto>>(classes),
                Success = true
            };
        }

        public async Task<Response> GetAsync(string classId)
        {
            var classModel = await _unit.Classes.Include(c => c.Teacher)
                                .FirstOrDefaultAsync(c => c.ClassId == classId);

            if (classModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any classes",
                    Success = false
                };
            }

            var classResDto = _mapper.Map<ClassResDto>(classModel);

            var userModel = await _userManager.FindByIdAsync(classModel.TeacherId);

            classResDto.Teacher!.Email = userModel!.Email;

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = classResDto,
                Success = true
            };
        }

        public async Task<Response> GetClassesWithTeacherAsync(string userId)
        {
            var isExistTeacher = _unit.Teachers.IsExist(t => t.UserId == userId);
            if (!isExistTeacher)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any teachers",
                    Success = false
                };
            }

            var classes = await _unit.Classes.GetClassesWithTeacherAsync(userId);

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<ClassResDto>>(classes),
                Success = true
            };
        }

        public async Task<Response> UpdateAsync(string classId, ClassDto model)
        {
            var classModel = _unit.Classes.GetById(classId);

            if (classModel == null)
            {
                return new Response()
                {
                    Message = "Can't find any classes",
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                };
            }

            if (classModel.TeacherId != model.TeacherId)
            {
                var isChangeSuccess = await _unit.Classes.ChangeTeacherAsync(classModel, model.TeacherId);
                if (!isChangeSuccess)
                {
                    return new Response()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Message = "Can't find any teachers",
                        Success = false
                    };
                }
            }

            if (model.Image != null)
            {
                var isChangeSuccess = await _unit.Classes.ChangeImageAsync(classModel, model.Image);

                if (!isChangeSuccess)
                {
                    return new Response()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Message = "Change image failed",
                        Success = false
                    };
                }
            }

            if (!model.StartDate.HasValue || !model.EndDate.HasValue)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Start and End is required"
                };
            }

            if (model.StartDate.HasValue && model.EndDate.HasValue)
            {
                if (model.StartDate.Value > model.EndDate.Value)
                {
                    return new Response()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Message = "Start time must be less than end time"
                    };
                }
            }

            var currentDate = DateOnly.FromDateTime(DateTime.Now);

            if (model.StartDate.Value >= currentDate)
            {
                classModel.Status = (int)ClassEnum.Waiting;
            }
            if (model.StartDate.Value <= currentDate && currentDate <= model.EndDate.Value)
            {
                classModel.Status = (int)ClassEnum.Opening;
            }

            if (model.EndDate.Value < currentDate)
            {
                classModel.Status = (int)ClassEnum.End;
            }

            classModel.StartDate = model.StartDate;
            classModel.EndDate = model.EndDate;
            classModel.RegisteredNum = model.RegisteredNum ?? 0;
            classModel.MaxNum = model.MaxNum ?? 0;
            classModel.Description = model.Description ?? classModel.Description;

            await _unit.CompleteAsync();

            if (classModel.Status == (int)ClassEnum.Opening)
            {
                var response = await _enrollService.HandleStartClassAsync(classId);
                if (!response.Success) return response;
            }

            if (classModel.Status == (int)ClassEnum.End)
            {
                var response = await _enrollService.HandleEndClassAsync(classId);
                if (!response.Success) return response;
            }


            return new Response()
            {
                Success = true,
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = ""
            };
        }

        public Task<bool> IsClassOfTeacherAsync(string userId, string classId)
        {
            var isValid = _unit.Classes.IsExist(c => c.ClassId == classId && c.TeacherId == userId);

            return Task.FromResult(isValid);
        }
    }
}
