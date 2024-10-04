using AutoMapper;
using EnglishCenter.Business.IServices;
using EnglishCenter.DataAccess.UnitOfWork;
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
        public Task<Response> ChangeAssignmentIdAsync(long id, long assignmentId)
        {
            throw new NotImplementedException();
        }

        public Task<Response> ChangeQuesAsync(long id, int type, long quesId)
        {
            throw new NotImplementedException();
        }

        public Task<Response> CreateAsync(AssignQuesDto model)
        {
            throw new NotImplementedException();
        }

        public Task<Response> DeleteAsync(long id)
        {
            throw new NotImplementedException();
        }

        public async Task<Response> GetAllAsync()
        {
            var models = _unit.AssignQues.GetAll();
            
            foreach(var model in models)
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
                Message = _mapper.Map<List<AssignQuesResDto>>(models),
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
                Message = _mapper.Map<List<AssignQuesResDto>>(assignQues),
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
                Message = _mapper.Map<AssignQuesResDto>(model),
                Success = true
            };
        }

        public Task<Response> UpdateAsync(long id, AssignQuesDto model)
        {
            throw new NotImplementedException();
        }
    }
}
