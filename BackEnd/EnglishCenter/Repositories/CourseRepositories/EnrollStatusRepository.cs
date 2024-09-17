using AutoMapper;
using EnglishCenter.Database;
using EnglishCenter.Models;
using EnglishCenter.Models.DTO;
using EnglishCenter.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.Repositories.CourseRepositories
{
    public class EnrollStatusRepository : IEnrollStatusRepository
    {
        private readonly EnglishCenterContext _context;
        private readonly IMapper _mapper;

        public EnrollStatusRepository(EnglishCenterContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public Response CreateEnrollStatus(EnrollStatusDto model)
        {
            var isExist = _context.EnrollStatuses.Any(s => s.StatusId == model.StatusId);

            if(isExist)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Enroll Status already existed",
                    Success = false
                };
            }

            var enrollModel = _mapper.Map<EnrollStatus>(model);
            _context.EnrollStatuses.Add(enrollModel);
            _context.SaveChangesAsync().Wait();

            return new Response()
            {
                Message = "",
                StatusCode = System.Net.HttpStatusCode.OK,
                Success = true
            };
        }

        public async Task<Response> DeleteEnrollStatusAsync(int enrollStatusId)
        {
            var enrollStatus = await _context.EnrollStatuses.FindAsync(enrollStatusId);

            if (enrollStatus == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any Enroll Status",
                    Success = false
                };
            }

            _context.EnrollStatuses.Remove(enrollStatus);
            _context.SaveChangesAsync().Wait();

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "",
                Success = true
            };
        }

        public async Task<Response> GetAllEnrollStatusAsync()
        {
            var enrollStatuses = await _context.EnrollStatuses.OrderBy(e => e.StatusId).ToListAsync();

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<EnrollStatusDto>>(enrollStatuses),
                Success = true
            };
        }

        public async Task<Response> GetEnrollStatusAsync(int enrollStatusId)
        {
            var enrollStatus = await _context.EnrollStatuses.FindAsync(enrollStatusId);
            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Success = true,
                Message = ""
            };
        }

        public async Task<Response> UpdateEnrollStatusAsync(int enrollStatusId, EnrollStatusDto model)
        {
            var enrollStatus = await _context.EnrollStatuses.FindAsync(enrollStatusId);

            if (enrollStatus == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any Enroll Status",
                    Success = false
                };
            }

            enrollStatus.Name = model.Name;
            _context.SaveChangesAsync().Wait();

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Success = true,
                Message = ""
            };
        }
    }
}
