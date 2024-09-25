using AutoMapper;
using EnglishCenter.Business.IServices;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.UnitOfWork;
using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.Business.Services.Courses
{
    public class EnrollStatusService : IEnrollStatusService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unit;

        public EnrollStatusService(IMapper mapper, IUnitOfWork unit)
        {
            _mapper = mapper;
            _unit = unit;
        }

        public async Task<Response> CreateAsync(EnrollStatusDto model)
        {
            var isExist = _unit.EnrollStatus.IsExist(x => x.StatusId == model.StatusId);
            if(isExist)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Success = false,
                    Message = "Enroll Status already existed"
                };
            }

            var enrollStatusModel = _mapper.Map<EnrollStatus>(model);
            _unit.EnrollStatus.Add(enrollStatusModel);
            await _unit.CompleteAsync();

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "",
                Success = true
            };
        }

        public async Task<Response> DeleteAsync(int enrollStatusId)
        {
            var enrollModel = _unit.EnrollStatus.GetById(enrollStatusId);

            if(enrollModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any enroll status",
                    Success = false
                };
            }

            _unit.EnrollStatus.Remove(enrollModel);
            await _unit.CompleteAsync();

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "",
                Success = true
            };
        }

        public  Task<Response> GetAllAsync()
        {
            var enrollStatusList = _unit.EnrollStatus.GetAll();

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<EnrollStatusDto>>(enrollStatusList),
                Success = true
            });
        }

        public Task<Response> GetAsync(int enrollStatusId)
        {
            var enrollModel = _unit.EnrollStatus.GetById(enrollStatusId);

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<EnrollStatusDto>(enrollModel),
                Success = true
            });
        }

        public async Task<Response> UpdateAsync(int enrollStatusId, EnrollStatusDto model)
        {
            var isExist = _unit.EnrollStatus.IsExist(x => x.StatusId == enrollStatusId);

            if (!isExist)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Success = false,
                    Message = "Can't find any enroll status"
                };
            }

            var isSuccess = await _unit.EnrollStatus.UpdateAsync(enrollStatusId, _mapper.Map<EnrollStatus>(model));

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
    }
}
