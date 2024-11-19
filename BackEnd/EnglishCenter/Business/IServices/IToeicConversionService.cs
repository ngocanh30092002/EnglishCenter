using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.Business.IServices
{
    public interface IToeicConversionService
    {
        public Task<Response> GetAllAsync();
        public Task<Response> GetAsync(int id);
        public Task<Response> GetBySectionAsync(string section);
        public Task<Response> CreateAsync(ToeicConversionDto model);
        public Task<Response> DeleteAsync(int id);
    }
}
