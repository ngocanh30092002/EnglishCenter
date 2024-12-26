using AutoMapper;
using EnglishCenter.Business.IServices;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.UnitOfWork;
using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;
using EnglishCenter.Presentation.Models.ResDTOs;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.Business.Services.Exams
{
    public class ToeicExamService : IToeicExamService
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;
        private readonly IQuesToeicService _queService;

        public ToeicExamService(IUnitOfWork unit, IMapper mapper, IQuesToeicService queService)
        {
            _unit = unit;
            _mapper = mapper;
            _queService = queService;
        }

        public async Task<Response> ChangeCodeAsync(long id, int code)
        {
            var toeicExam = _unit.ToeicExams.GetById(id);
            if (toeicExam == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any toeic examination",
                    Success = false
                };
            }

            if (code < 0)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Code must be greater than 0",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.ToeicExams.ChangeCodeAsync(toeicExam, code);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't change code",
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

        public async Task<Response> ChangeCompleteNumAsync(long id, int num)
        {
            var toeicExam = _unit.ToeicExams.GetById(id);
            if (toeicExam == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any toeic examination",
                    Success = false
                };
            }

            if (num < 0)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Number must be greater than 0",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.ToeicExams.ChangeCompleteNumAsync(toeicExam, num);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't change completed number",
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

        public async Task<Response> ChangeMinutesAsync(long id, int minutes)
        {
            var toeicExam = _unit.ToeicExams.GetById(id);
            if (toeicExam == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any toeic examination",
                    Success = false
                };
            }

            if (minutes < 0)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Minutes must be greater than 0",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.ToeicExams.ChangeMinutesAsync(toeicExam, minutes);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't change minutes",
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

        public async Task<Response> ChangeNameAsync(long id, string newName)
        {
            var toeicExam = _unit.ToeicExams.GetById(id);
            if (toeicExam == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any toeic examination",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.ToeicExams.ChangeNameAsync(toeicExam, newName);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't change name",
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

        public async Task<Response> ChangePointAsync(long id, int point)
        {
            var toeicExam = _unit.ToeicExams.GetById(id);
            if (toeicExam == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any toeic examination",
                    Success = false
                };
            }

            if (point < 0)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Point must be greater than 0",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.ToeicExams.ChangePointAsync(toeicExam, point);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't change point",
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

        public async Task<Response> ChangeYearAsync(long id, int year)
        {
            var toeicExam = _unit.ToeicExams.GetById(id);
            if (toeicExam == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any toeic examination",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.ToeicExams.ChangeYearAsync(toeicExam, year);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't change year",
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

        public async Task<Response> CreateAsync(ToeicExamDto model)
        {
            var isExistCode = _unit.ToeicExams.IsExist(t => t.Code == model.Code && t.Year == model.Year);
            if (isExistCode)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't add same code in same year",
                    Success = false
                };
            }

            var toeicModel = _mapper.Map<ToeicExam>(model);
            toeicModel.DirectionId = 1;

            _unit.ToeicExams.Add(toeicModel);
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
            var toeicExam = _unit.ToeicExams.GetById(id);
            if (toeicExam == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any toeic examination",
                    Success = false
                };
            }

            var queIds = _unit.QuesToeic
                              .Find(q => q.ToeicId == id)
                              .Select(q => q.QuesId)
                              .ToList();
            await _unit.BeginTransAsync();

            try
            {
                foreach (var queId in queIds)
                {
                    var deleteRes = await _queService.DeleteAsync(queId);
                    if (!deleteRes.Success) return deleteRes;
                }

                _unit.ToeicExams.Remove(toeicExam);

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

        public async Task<Response> GetAllAsync()
        {
            var toeicExams = _unit.ToeicExams.GetAll().OrderByDescending(e => e.Year);

            var resDtos = _mapper.Map<List<ToeicExamResDto>>(toeicExams);

            foreach (var resDto in resDtos)
            {
                resDto.IsFull = await _queService.IsFullAsync(resDto.ToeicId!.Value);
            }

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = resDtos,
                Success = true
            };
        }

        public Task<Response> GetAsync(long id)
        {
            var toeicExam = _unit.ToeicExams.GetById(id);

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<ToeicExamResDto>(toeicExam),
                Success = true
            });
        }

        public async Task<Response> GetToeicDirectionAsync(long id)
        {
            var toeicExam = await _unit.ToeicExams
                                       .Include(e => e.ToeicDirection)
                                       .FirstOrDefaultAsync(s => s.ToeicId == id);

            if (toeicExam == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any toeic examinations",
                    Success = false
                };
            }

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<ToeicDirectionResDto>(toeicExam.ToeicDirection),
                Success = true
            };
        }

        public async Task<Response> UpdateAsync(long id, ToeicExamDto model)
        {
            var toeicExam = _unit.ToeicExams.GetById(id);
            if (toeicExam == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any toeic examination",
                    Success = false
                };
            }

            await _unit.BeginTransAsync();

            try
            {
                if (toeicExam.Name != model.Name)
                {
                    var response = await ChangeNameAsync(id, model.Name);
                    if (!response.Success) return response;
                }

                if (toeicExam.Year != model.Year)
                {
                    var response = await ChangeYearAsync(id, model.Year);
                    if (!response.Success) return response;
                }

                if (toeicExam.Code != model.Code)
                {
                    var response = await ChangeCodeAsync(id, model.Code);
                    if (!response.Success) return response;
                }

                if (model.Point.HasValue && toeicExam.Point != model.Point)
                {
                    var response = await ChangePointAsync(id, model.Point.Value);
                    if (!response.Success) return response;
                }

                if (model.Completed_Num.HasValue && toeicExam.CompletedNum != model.Completed_Num)
                {
                    var response = await ChangeCompleteNumAsync(id, model.Completed_Num.Value);
                    if (!response.Success) return response;
                }

                if (model.TimeMinutes.HasValue && toeicExam.TimeMinutes != model.TimeMinutes)
                {
                    var response = await ChangeMinutesAsync(id, model.TimeMinutes.Value);
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
