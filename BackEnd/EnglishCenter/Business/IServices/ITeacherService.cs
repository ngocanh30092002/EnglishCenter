using EnglishCenter.Presentation.Models;

namespace EnglishCenter.Business.IServices
{
    public interface ITeacherService
    {
        public Response GetFullName(string userId);
        //public Task<Response> AcceptedStudentAsync(long enrollId);
    }
}
