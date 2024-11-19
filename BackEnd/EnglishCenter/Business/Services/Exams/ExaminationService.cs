using AutoMapper;
using EnglishCenter.Business.IServices;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.UnitOfWork;
using EnglishCenter.Presentation.Global.Enum;
using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.Business.Services.Exams
{
    public class ExaminationService : IExaminationService
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;

        public ExaminationService(IUnitOfWork unit, IMapper mapper)
        {
            _unit = unit;
            _mapper = mapper;
        }

        public async Task<Response> ChangeCourseContentAsync(long id, long contentId)
        {
            var examModel = _unit.Examinations.GetById(id);
            if (examModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any examinations",
                    Success = false
                };
            }


            var isChangeSuccess = await _unit.Examinations.ChangeCourseContentAsync(examModel, contentId);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't change course content",
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

        public async Task<Response> ChangeDescriptionAsync(long id, string description)
        {
            var examModel = _unit.Examinations.GetById(id);
            if (examModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any examinations",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.Examinations.ChangeDescriptionAsync(examModel, description);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't change description",
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

        public async Task<Response> ChangeTimeAsync(long id, string timeStr)
        {
            var examModel = _unit.Examinations.GetById(id);
            if (examModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any examinations",
                    Success = false
                };
            }

            if (!TimeOnly.TryParse(timeStr, out var timeOnly))
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Time is invalid",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.Examinations.ChangeTimeAsync(examModel, timeOnly);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't change time",
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

        public async Task<Response> ChangeTitleAsync(long id, string title)
        {
            var examModel = _unit.Examinations.GetById(id);
            if (examModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any examinations",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.Examinations.ChangeTitleAsync(examModel, title);
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

        public async Task<Response> ChangeToeicAsync(long id, long toeicId)
        {
            var examModel = _unit.Examinations.GetById(id);
            if (examModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any examinations",
                    Success = false
                };
            }


            var isChangeSuccess = await _unit.Examinations.ChangeToeicAsync(examModel, toeicId);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't change toeic id",
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

        public async Task<Response> CreateAsync(ExaminationDto model)
        {
            var isExistCourseContent = _unit.CourseContents
                                            .IsExist(c => c.ContentId == model.ContentId && c.Type != (int)CourseContentTypeEnum.Normal);
            if (!isExistCourseContent)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any course contents",
                    Success = false
                };
            }

            var isExistToeic = _unit.ToeicExams.IsExist(t => t.ToeicId == model.ToeicId);
            if (!isExistToeic)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any toeic examination",
                    Success = false
                };
            }

            if (!TimeOnly.TryParse(model.Time, out var timeOnly))
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Time is invalid",
                    Success = false
                };
            }

            var examModel = _mapper.Map<Examination>(model);
            examModel.Time = timeOnly;

            _unit.Examinations.Add(examModel);
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
            var examModel = _unit.Examinations.GetById(id);
            if (examModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any examinations",
                    Success = false
                };
            }

            var isExistProcess = _unit.LearningProcesses.IsExist(p => p.ExamId == examModel.ExamId);
            if (isExistProcess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't remove examination because a process already exists.",
                    Success = false
                };
            }

            _unit.Examinations.Remove(examModel);

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
            var examModels = _unit.Examinations.GetAll();

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<ExaminationDto>>(examModels),
                Success = true
            });
        }

        public Task<Response> GetAsync(long id)
        {
            var examModel = _unit.Examinations.GetById(id);
            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<ExaminationDto>(examModel),
                Success = true
            });
        }

        public async Task<Response> UpdateAsync(long id, ExaminationDto model)
        {
            var examModel = _unit.Examinations.GetById(id);
            if (examModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any examinations",
                    Success = false
                };
            }

            await _unit.BeginTransAsync();
            try
            {
                if (examModel.ContentId != model.ContentId)
                {
                    var response = await ChangeCourseContentAsync(id, model.ContentId);
                    if (!response.Success) return response;
                }

                if (examModel.ToeicId != model.ToeicId)
                {
                    var response = await ChangeToeicAsync(id, model.ToeicId);
                    if (!response.Success) return response;
                }

                if (!string.IsNullOrEmpty(model.Time) && examModel.Time.ToString("hh:mm:ss") != model.Time)
                {
                    var response = await ChangeTimeAsync(id, model.Time);
                    if (!response.Success) return response;
                }

                if (examModel.Title != model.Title)
                {
                    var response = await ChangeTitleAsync(id, model.Title ?? "");
                    if (!response.Success) return response;
                }

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
