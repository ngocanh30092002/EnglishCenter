using AutoMapper;
using EnglishCenter.Business.IServices;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.UnitOfWork;
using EnglishCenter.Presentation.Global.Enum;
using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.Business.Services.Courses
{
    public class ToeicConversionService : IToeicConversionService
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;

        public ToeicConversionService(IUnitOfWork unit, IMapper mapper)
        {
            _unit = unit;
            _mapper = mapper;
        }

        public async Task<Response> CreateAsync(ToeicConversionDto model)
        {
            var isExistRecords = _unit.ToeicConversion.IsExist(t => t.NumberCorrect == model.NumberCorrect && t.Section == model.Section.ToString());
            if (isExistRecords)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't create same number correct with section",
                    Success = false
                };
            }

            if(!Enum.TryParse(model.Section, true, out ToeicEnum section))
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Invalid Section",
                    Success = false
                };
            }

            var toeicEntity = _mapper.Map<ToeicConversion>(model);
            toeicEntity.Section = section.ToString();

            _unit.ToeicConversion.Add(toeicEntity);

            await _unit.CompleteAsync();
            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "",
                Success = true
            };
        }

        public async Task<Response> DeleteAsync(int id)
        {
            var model = _unit.ToeicConversion.GetById(id);

            if (model == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any records",
                    Success = false
                };
            }

            _unit.ToeicConversion.Remove(model);

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
            var models = _unit.ToeicConversion.GetAll();

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<ToeicConversionDto>>(models),
                Success = true
            });
        }

        public Task<Response> GetAsync(int id)
        {
            var model = _unit.ToeicConversion.GetById(id);

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<ToeicConversionDto>(model),
                Success = true
            });

        }

        public Task<Response> GetBySectionAsync(string section)
        {
            var models = _unit.ToeicConversion.Find(t => t.Section == section);

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<ToeicConversionDto>>(models),
                Success = true
            });
        }
    }
}
