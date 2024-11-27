using EnglishCenter.Presentation.Models;

namespace EnglishCenter.Business.IServices
{
    public interface IPeriodService
    {
        public Task<Response> GetAllAsync();
    }
}
