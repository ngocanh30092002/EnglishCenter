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

namespace EnglishCenter.Business.Services.Classes
{
    public class SubmissionFileService : ISubmissionFileService
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<User> _userManager;

        public SubmissionFileService(IUnitOfWork unit, IMapper mapper, IWebHostEnvironment webHostEnvironment, UserManager<User> userManager)
        {
            _unit = unit;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
        }

        public async Task<Response> ChangeFilePathAsync(long id, IFormFile file)
        {
            var fileModel = _unit.SubmissionFiles.GetById(id);
            if (fileModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any submission files",
                    Success = false
                };
            }

            var previousFile = Path.Combine(_webHostEnvironment.WebRootPath, fileModel.FilePath ?? "");
            if (File.Exists(previousFile))
            {
                File.Delete(previousFile);
            }

            var folderPath = Path.Combine(_webHostEnvironment.WebRootPath, "submission_tasks", fileModel.SubmissionTaskId.ToString());
            var fileName = $"{DateTime.Now.Ticks}_{file.FileName}";
            var result = await UploadHelper.UploadFileAsync(file, folderPath, fileName);
            if (!string.IsNullOrEmpty(result))
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = result,
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.SubmissionFiles.ChangeFilePathAsync(fileModel, Path.Combine("submission_tasks", fileModel.SubmissionTaskId.ToString(), fileName));
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't change file path",
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

        public async Task<Response> ChangeLinkUrlAsync(long id, string newLinkUrl)
        {
            var fileModel = _unit.SubmissionFiles.GetById(id);
            if (fileModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any submission files",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.SubmissionFiles.ChangeLinkUrlAsync(fileModel, newLinkUrl);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't change link url",
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

        public async Task<Response> ChangeSubmissionTaskAsync(long id, long submitTaskId)
        {
            var fileModel = _unit.SubmissionFiles.GetById(id);
            if (fileModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any submission files",
                    Success = false
                };
            }

            var submissionModel = _unit.SubmissionTasks.GetById(submitTaskId);
            if (submissionModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any submission tasks",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.SubmissionFiles.ChangeSubmissionTaskAsync(fileModel, submitTaskId);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't change submission",
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
            var fileModel = _unit.SubmissionFiles.GetById(id);
            if (fileModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any submission files",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.SubmissionFiles.ChangeUploadByAsync(fileModel, uploadBy);
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

        public async Task<Response> CreateAsync(string userId, SubmissionFileDto model)
        {
            if (model.File == null && string.IsNullOrEmpty(model.LinkUrl))
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "File or Link url must be required",
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

            var submissionTask = _unit.SubmissionTasks.GetById(model.SubmissionTaskId);
            if (submissionTask == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any submission tasks",
                    Success = false
                };
            }

            var enrollModel = _unit.Enrollment.GetById(model.EnrollId);
            if (enrollModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any enrollments",
                    Success = false
                };
            }

            if (enrollModel.StatusId != (int)EnrollEnum.Ongoing)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Status enrollment is invalid",
                    Success = false
                };
            }

            var fileEntity = new SubmissionFile();


            if (model.File != null)
            {
                var folderPath = Path.Combine(_webHostEnvironment.WebRootPath, "submission_tasks", submissionTask.SubmissionId.ToString());
                var fileName = $"{DateTime.Now.Ticks}_{model.File.FileName}";
                var result = await UploadHelper.UploadFileAsync(model.File, folderPath, fileName);
                if (!string.IsNullOrEmpty(result))
                {
                    return new Response()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Message = result,
                        Success = false
                    };
                }

                fileEntity.FilePath = Path.Combine("submission_tasks", submissionTask.SubmissionId.ToString(), fileName);
            }


            var roles = await _userManager.GetRolesAsync(userModel);
            if (roles.Any(r => r == AppRole.STUDENT || r == AppRole.ADMIN))
            {
                var student = _unit.Students.GetById(userModel.Id);
                fileEntity.UploadBy = student.FirstName + " " + student.LastName;
            }
            else
            {
                var teacher = _unit.Teachers.GetById(userModel.Id);
                fileEntity.UploadBy = teacher.FirstName + " " + teacher.LastName;
            }

            fileEntity.LinkUrl = model.LinkUrl;
            fileEntity.UploadAt = DateTime.Now;
            fileEntity.Status = (submissionTask.StartTime <= fileEntity.UploadAt && fileEntity.UploadAt <= submissionTask.EndTime) ? (int)SubmissionFileEnum.OnTime : (int)SubmissionFileEnum.Late;
            fileEntity.SubmissionTaskId = submissionTask.SubmissionId;
            fileEntity.EnrollId = model.EnrollId;

            _unit.SubmissionFiles.Add(fileEntity);
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
            var fileModel = _unit.SubmissionFiles.GetById(id);
            if (fileModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any submission files",
                    Success = false
                };
            }

            var previousFile = Path.Combine(_webHostEnvironment.WebRootPath, fileModel.FilePath ?? "");
            if (File.Exists(previousFile))
            {
                File.Delete(previousFile);
            }


            _unit.SubmissionFiles.Remove(fileModel);
            await _unit.CompleteAsync();

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "",
                Success = true
            };
        }

        public async Task<Response> HandleUploadMoreFilesAsync(string userId, SubmissionFileDto model)
        {
            if (model.Files == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Files is required",
                    Success = false
                };
            }

            await _unit.BeginTransAsync();

            try
            {
                foreach (var file in model.Files)
                {
                    model.File = file;
                    var createRes = await CreateAsync(userId, model);
                    if (!createRes.Success) return createRes;
                }

                await _unit.CompleteAsync();
                await _unit.CommitTransAsync();

                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Message = "",
                    Success = true
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
            var models = _unit.SubmissionFiles.GetAll();

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<SubmissionFileResDto>>(models),
                Success = true
            });
        }

        public Task<Response> GetAsync(long id)
        {
            var model = _unit.SubmissionFiles.GetById(id);

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<SubmissionFileResDto>(model),
                Success = true
            });
        }

        public Task<Response> GetByEnrollAndIdAsync(long enrollId, long submissionTaskId)
        {
            var files = _unit.SubmissionFiles
                             .Find(f => f.EnrollId == enrollId && f.SubmissionTaskId == submissionTaskId)
                             .ToList();

            var resDtos = _mapper.Map<List<SubmissionFileResDto>>(files);

            foreach (var resDto in resDtos)
            {
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, resDto.FilePath ?? "");

                if (File.Exists(filePath))
                {
                    FileInfo fileInfo = new FileInfo(filePath);
                    long fileSizeInBytes = fileInfo.Length;

                    double fileSizeInKB = fileSizeInBytes / 1024.0;
                    double fileSizeInMB = fileSizeInKB / 1024.0;

                    resDto.FileSize = $"{Math.Round(fileSizeInMB, 2)} MB";
                }
            }



            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = resDtos,
                Success = true
            });
        }

        public async Task<Response> UpdateAsync(long id, SubmissionFileDto model)
        {
            var fileModel = _unit.SubmissionFiles.GetById(id);
            if (fileModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any submission files",
                    Success = false
                };
            }

            await _unit.BeginTransAsync();

            try
            {
                if (model.File != null)
                {
                    var res = await ChangeFilePathAsync(id, model.File);
                    if (!res.Success) return res;
                }

                if (!string.IsNullOrEmpty(model.LinkUrl) && fileModel.LinkUrl != model.LinkUrl)
                {
                    var res = await ChangeLinkUrlAsync(id, model.LinkUrl);
                    if (!res.Success) return res;
                }

                if (fileModel.SubmissionTaskId != model.SubmissionTaskId)
                {
                    var res = await ChangeSubmissionTaskAsync(id, model.SubmissionTaskId);
                    if (!res.Success) return res;
                }

                await _unit.CompleteAsync();
                await _unit.CommitTransAsync();

                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Message = "",
                    Success = true
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
    }
}
