﻿using AutoMapper;
using EnglishCenter.Business.IServices;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.UnitOfWork;
using EnglishCenter.Presentation.Global.Enum;
using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;
using EnglishCenter.Presentation.Models.ResDTOs;

namespace EnglishCenter.Business.Services.Assignments
{
    public class AssignQuesService : IAssignQuesService
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;

        public AssignQuesService(IUnitOfWork unit, IMapper mapper)
        {
            _unit = unit;
            _mapper = mapper;
        }

        public async Task<Response> ChangeAssignmentIdAsync(long id, long assignmentId)
        {
            var assignQuesModel = _unit.AssignQues.GetById(id);
            if(assignQuesModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any Assignment Question",
                    Success = false
                };
            }

            var isExistAssignment = _unit.Assignments.IsExist(a => a.AssignmentId == assignmentId);
            if (!isExistAssignment)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any assignments",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.AssignQues.ChangeAssignmentIdAsync(assignQuesModel, assignmentId);
            if (!isChangeSuccess)
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

        public async Task<Response> ChangeQuesAsync(long id, int type, long quesId)
        {
            var assignQuesModel = _unit.AssignQues.GetById(id);
            if(assignQuesModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any Assignment Question",
                    Success = false
                };
            }

            if(!Enum.IsDefined(typeof(QuesTypeEnum), type))
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Invalid type",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.AssignQues.ChangeQuesAsync(assignQuesModel, (QuesTypeEnum)type, quesId);
            if (!isChangeSuccess)
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

        public async Task<Response> CreateAsync(AssignQueDto model)
        {
            var isExistAssignment = _unit.Assignments.IsExist(a => a.AssignmentId == model.AssignmentId);
            if (!isExistAssignment)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any assignments",
                    Success = false
                };
            }

            if(!Enum.IsDefined(typeof(QuesTypeEnum), model.Type))
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Invalid Type",
                    Success = false
                };
            }

            var isExistQuesType = await _unit.AssignQues.IsExistQuesIdAsync((QuesTypeEnum)model.Type, model.QuesId);
            if (!isExistQuesType) 
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any question",
                    Success = false
                };
            }

            var assignModel = _mapper.Map<AssignQue>(model);

            _unit.AssignQues.Add(assignModel);
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
            var assignQuesModel = _unit.AssignQues.GetById(id);
            if(assignQuesModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any Assignment Question",
                    Success = false
                };
            }

            _unit.AssignQues.Remove(assignQuesModel);
            await _unit.CompleteAsync();
            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "",
                Success = true
            };
        }

        public async Task<Response> GetAllAsync()
        {
            var models = _unit.AssignQues.GetAll();

            foreach (var model in models)
            {
                var isSuccess = await _unit.AssignQues.LoadQuestionAsync(model);
                if (!isSuccess)
                {
                    return new Response()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Message = "Load question fail",
                        Success = false
                    };
                }
            }

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<AssignQueResDto>>(models),
                Success = true
            };
        }

        public async Task<Response> GetByAssignmentAsync(long assignmentId)
        {
            var isExist = _unit.Assignments.IsExist(a => a.AssignmentId == assignmentId);
            if (!isExist)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any assignments",
                    Success = false
                };
            }

            var assignQues = await _unit.AssignQues.GetByAssignmentAsync(assignmentId);

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<AssignQueResDto>>(assignQues),
                Success = true
            };
        }

        public async Task<Response> GetAsync(long id)
        {
            var model = _unit.AssignQues.GetById(id);

            await _unit.AssignQues.LoadQuestionAsync(model);
            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<AssignQueResDto>(model),
                Success = true
            };
        }

        public async Task<Response> UpdateAsync(long id, AssignQueDto model)
        {
            var isSuccess = await _unit.AssignQues.UpdateAsync(id, model);
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
    }
}