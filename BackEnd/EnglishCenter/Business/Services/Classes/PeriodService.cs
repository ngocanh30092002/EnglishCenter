using AutoMapper;
using EnglishCenter.Business.IServices;
using EnglishCenter.DataAccess.UnitOfWork;
using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.ResDTOs;

namespace EnglishCenter.Business.Services.Classes
{
    public class PeriodService : IPeriodService
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;

        public PeriodService(IUnitOfWork unit, IMapper mapper)
        {
            _unit = unit;
            _mapper = mapper;
        }

        public Task<Response> GetAllAsync()
        {
            var models = _unit.Periods.GetAll();

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<PeriodResDto>>(models),
                Success = true
            });
        }
    }
}
