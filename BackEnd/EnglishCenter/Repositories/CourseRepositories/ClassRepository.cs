using AutoMapper;
using EnglishCenter.Database;
using EnglishCenter.Helpers;
using EnglishCenter.Models;
using EnglishCenter.Models.DTO;
using EnglishCenter.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.Repositories.CourseRepositories
{
    public class ClassRepository : IClassRepository
    {
        private readonly EnglishCenterContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ClassRepository(EnglishCenterContext context , IMapper mapper, IWebHostEnvironment webHostEnvironment) 
        {
            _context = context;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<Response> ChangeCourseAsync(string classId, string courseId)
        {
            var classModel = await _context.Classes.FindAsync(classId);
            if (classModel == null)
            {
                return new Response()
                {
                    Message = "Can't find any classes",
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                };
            }

            var course = await _context.Courses.FindAsync(courseId);
            if (course == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any courses"
                };
            }

            classModel.CourseId = courseId;
            await _context.SaveChangesAsync();

            return new Response()
            {
                Success = true,
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = ""
            };
        }

        public async Task<Response> ChangeEndTimeAsync(string classId, DateOnly endTime)
        {
            var classModel = await _context.Classes.FindAsync(classId);

            if (classModel == null)
            {
                return new Response()
                {
                    Message = "Can't find any classes",
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                };
            }

            if (classModel.StartDate.HasValue)
            {
                if(classModel.StartDate.Value > endTime)
                {
                    return new Response()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Success = false,
                        Message = "EndTime must be greater than StartTime"
                    };
                }
            }

            classModel.EndDate = endTime;
            await _context.SaveChangesAsync();

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "",
                Success = true
            };
        }

        public async Task<Response> ChangeImageAsync(string classId, IFormFile image)
        {
            var classModel = await _context.Classes.FindAsync(classId);

            if (classModel == null)
            {
                return new Response()
                {
                    Message = "Can't find any classes",
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                };
            }

            var oldPathImage = Path.Combine(_webHostEnvironment.WebRootPath, classModel.Image);
            if (File.Exists(oldPathImage))
            {
                File.Delete(oldPathImage);
            }

            var uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "classes", "images");
            var fileName = $"{DateTime.Now.Ticks}_{image.FileName}";
            var result = await UploadHelper.UploadFileAsync(image, uploadFolder, fileName);

            if (!string.IsNullOrEmpty(result))
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = result
                };
            }

            classModel.Image = Path.Combine("classes", "images", fileName);
            await _context.SaveChangesAsync();

            return new Response()
            {
                Success = true,
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = ""
            };
        }

        public async Task<Response> ChangeMaxNumAsync(string classId, int maxNum)
        {
            var classModel = await _context.Classes.FindAsync(classId);

            if (classModel == null)
            {
                return new Response()
                {
                    Message = "Can't find any classes",
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                };
            }

            if(maxNum < 0)
            {
                return new Response()
                {
                    Message = "MaxNum must be greater than 0",
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };
            }

            classModel.MaxNum = maxNum;
            await _context.SaveChangesAsync();

            return new Response()
            {
                Success = true,
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = ""
            };
        }

        public async Task<Response> ChangeStartTimeAsync(string classId, DateOnly startTime)
        {
            var classModel = await _context.Classes.FindAsync(classId);

            if (classModel == null)
            {
                return new Response()
                {
                    Message = "Can't find any classes",
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                };
            }

            if (classModel.EndDate.HasValue)
            {
                if(classModel.EndDate.Value < startTime)
                {
                    return new Response()
                    {
                        Message = "Start time must be less than end time",
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                    };
                }
            }

            classModel.StartDate = startTime;
            await _context.SaveChangesAsync();

            return new Response()
            {
                Success = true,
                Message = "",
                StatusCode = System.Net.HttpStatusCode.OK
            };
        }

        public async Task<Response> CreateClassAsync(ClassDto model)
        {
            var course = await _context.Courses.FindAsync(model.CourseId);
            if(course == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any courses"
                };
            }

            var isExistClass = _context.Classes.Any(c => c.ClassId == model.ClassId);
            if(isExistClass)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Class already exists",
                };
            }

            // Todo: Chưa tạo đến giáo viên
            var isExistTeacher = _context.Students.Any(s => s.UserId == model.TeacherId);
            if (!isExistTeacher)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any teachers"
                };
            }

            if(model.StartDate.HasValue && model.EndDate.HasValue)
            {
                if(model.StartDate.Value >  model.EndDate.Value)
                {
                    return new Response()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Message = "Start time must be less than end time"
                    };
                }
            }

            var classModel = _mapper.Map<Class>(model);

            if(model.Image != null)
            {
                var uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "classes", "images");
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

                classModel.Image = Path.Combine("classes", "images", fileName);
            }

            _context.Classes.Add(classModel);
            await _context.SaveChangesAsync();

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Success = true,
                Message = ""
            };
        }

        public async Task<Response> DeleteClassAsync(string classId)
        {
            var classModel = await _context.Classes.FindAsync(classId);

            if(classModel == null)
            {
                return new Response()
                {
                    Success = false,
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any classes"
                };
            }

            _context.Classes.Remove(classModel);
            await _context.SaveChangesAsync();

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Success = true,
                Message = ""
            };
        }

        public async Task<Response> GetClassAsync(string classId)
        {
            var classModel = await _context.Classes.FindAsync(classId);

            return new Response()
            {
                Message = _mapper.Map<ClassDto>(classModel),
                StatusCode = System.Net.HttpStatusCode.OK,
                Success = true,
            };
        }

        public async Task<Response> GetClassesAsync()
        {
            var classes = await _context.Classes.ToListAsync();

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Success = true,
                Message = _mapper.Map<List<ClassDto>>(classes)
            };
        }

        public async Task<Response> GetClassesWithTeacherAsync(string teacherId)
        {
            var classes = await _context.Classes.Where(c => c.TeacherId == teacherId).ToListAsync();

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<ClassDto>>(classes),
                Success = true
            };
        }

        public async Task<Response> UpdateClassAsync(string classId, ClassDto model)
        {
            var classModel = await _context.Classes.FindAsync(classId);

            if (classModel == null)
            {
                return new Response()
                {
                    Message = "Can't find any classes",
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                };
            }

            if(classModel.CourseId != model.CourseId)
            {
                var isExistCourse = _context.Courses.Any(c => c.CourseId == model.CourseId);
                if (!isExistCourse)
                {
                    return new Response()
                    {
                        Message = "Can't find any courses",
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                    };
                }

                classModel.CourseId = model.CourseId;
            }

            if(model.Image != null)
            {
                var changeImageRes =  await ChangeImageAsync(classId, model.Image);

                if (!changeImageRes.Success)
                {
                    return changeImageRes;
                }
            }

            if(model.StartDate.HasValue && model.EndDate.HasValue)
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

            classModel.StartDate = model.StartDate;
            classModel.EndDate = model.EndDate;
            classModel.RegisteredNum = model.RegisteredNum;
            classModel.MaxNum = model.MaxNum;

            _context.Classes.Update(classModel);
            await _context.SaveChangesAsync();

            return new Response()
            {
                Success = true,
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = ""
            };
        }
    }
}
