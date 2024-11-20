using AutoMapper;
using EnglishCenter.Business.IServices;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.UnitOfWork;
using EnglishCenter.Presentation.Global;
using EnglishCenter.Presentation.Helpers;
using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;
using EnglishCenter.Presentation.Models.ResDTOs;
using Microsoft.AspNetCore.Identity;

namespace EnglishCenter.Business.Services.Classes
{
    public class ClassMaterialService : IClassMaterialService
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<User> _userManager;

        public ClassMaterialService(IUnitOfWork unit, IMapper mapper, IWebHostEnvironment webHostEnvironment, UserManager<User> userManager)
        {
            _unit = unit;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
        }

        public async Task<Response> ChangeFilePathAsync(long id, IFormFile file)
        {
            var materialModel = _unit.ClassMaterials.GetById(id);
            if (materialModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any class materials",
                    Success = false
                };
            }

            var previousFile = Path.Combine(_webHostEnvironment.WebRootPath, materialModel.FilePath);
            if (File.Exists(previousFile))
            {
                File.Delete(previousFile);
            }

            string classId = string.Empty;

            if (!string.IsNullOrEmpty(materialModel.ClassId))
            {
                classId = materialModel.ClassId;
            }

            if (materialModel.LessonId.HasValue)
            {
                var lessonModel = _unit.Lessons.GetById(materialModel.LessonId.Value);

                classId = lessonModel.ClassId;
            }

            var folderPath = Path.Combine(_webHostEnvironment.WebRootPath, "materials", classId);
            var result = await UploadHelper.UploadFileAsync(file, folderPath, file.FileName);
            if (!string.IsNullOrEmpty(result))
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = result,
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.ClassMaterials.ChangeFilePathAsync(materialModel, Path.Combine("materials", classId, file.FileName));
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't change file in class materials",
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

        public async Task<Response> ChangeTitleAsync(long id, string newTitle)
        {
            var materialModel = _unit.ClassMaterials.GetById(id);
            if (materialModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any class materials",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.ClassMaterials.ChangeTitleAsync(materialModel, newTitle);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't change title",
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

        public async Task<Response> ChangeUploadByAsync(long id, string uploadBy)
        {
            var materialModel = _unit.ClassMaterials.GetById(id);
            if (materialModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any class materials",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.ClassMaterials.ChangeUploadByAsync(materialModel, uploadBy);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't change upload by",
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

        public async Task<Response> CreateAsync(string userId, ClassMaterialDto model)
        {
            if (string.IsNullOrEmpty(model.Title))
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Title is required",
                    Success = false
                };
            }

            if (string.IsNullOrEmpty(model.ClassId) && !model.LessonId.HasValue)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Materials must belong to only a class or a lesson",
                    Success = false
                };
            }

            if (!string.IsNullOrEmpty(model.ClassId) && model.LessonId.HasValue)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Materials must belong to only a class or a lesson",
                    Success = false
                };
            }

            var classMaterial = new ClassMaterial();
            var classId = string.Empty;
            if (model.LessonId.HasValue)
            {
                var lessonModel = _unit.Lessons.GetById(model.LessonId.Value);
                if (lessonModel == null)
                {
                    return new Response()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Message = "Can't find any lessons",
                        Success = false
                    };
                }

                classMaterial.LessonId = model.LessonId.Value;
                classId = lessonModel.ClassId;
            }

            if (!string.IsNullOrEmpty(model.ClassId))
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

                classMaterial.ClassId = classId;
                classId = classModel.ClassId;
            }

            if (model.File == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "File is required",
                    Success = false
                };
            }

            var folderPath = Path.Combine(_webHostEnvironment.WebRootPath, "materials", classId);
            var result = await UploadHelper.UploadFileAsync(model.File, folderPath, model.File.FileName);
            if (!string.IsNullOrEmpty(result))
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = result,
                    Success = false
                };
            }

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

            var roles = await _userManager.GetRolesAsync(userModel);

            if (roles.Any(r => r == AppRole.STUDENT || r == AppRole.ADMIN))
            {
                var student = _unit.Students.GetById(userModel.Id);
                classMaterial.UploadBy = student.FirstName + " " + student.LastName;
            }
            else
            {
                var teacher = _unit.Teachers.GetById(userModel.Id);
                classMaterial.UploadBy = teacher.FirstName + " " + teacher.LastName;
            }

            classMaterial.UploadAt = DateTime.Now;
            classMaterial.Title = model.Title;
            classMaterial.FilePath = Path.Combine("materials", classId, model.File.FileName);

            _unit.ClassMaterials.Add(classMaterial);
            await _unit.CompleteAsync();

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "",
                Success = true
            };
        }

        public async Task<Response> DeleteAsync(long id)
        {
            var materialModel = _unit.ClassMaterials.GetById(id);
            if (materialModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any class materials",
                    Success = false
                };
            }

            var previousFile = Path.Combine(_webHostEnvironment.WebRootPath, materialModel.FilePath);
            if (File.Exists(previousFile))
            {
                File.Delete(previousFile);
            }

            _unit.ClassMaterials.Remove(materialModel);
            await _unit.CompleteAsync();

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "",
                Success = true
            };
        }

        public Task<Response> GetAllAsync()
        {
            var models = _unit.ClassMaterials.GetAll();

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<ClassMaterialResDto>>(models),
                Success = true
            });
        }

        public Task<Response> GetAsync(long id)
        {
            var model = _unit.ClassMaterials.GetById(id);

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<ClassMaterialResDto>(model),
                Success = true
            });
        }
    }
}
