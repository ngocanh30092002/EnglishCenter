using EnglishCenter.Database;
using EnglishCenter.Models;
using EnglishCenter.Repositories.IRepositories;

namespace EnglishCenter.Repositories.AuthenticationRepositories
{
    public class TeacherRepository : ITeacherRepository
    {
        private readonly EnglishCenterContext _context;
        private readonly IEnrollmentRepository _enrollRepo;

        public TeacherRepository(EnglishCenterContext context, IEnrollmentRepository enrollRepo) 
        {
            _context = context;
            _enrollRepo = enrollRepo;
        }

        public async Task<Response> GetFullNameAsync(string userId)
        {
            var user = await _context.Students.FindAsync(userId);

            if(user == null)
            {
                return new Response()
                {
                    Success = false,
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any teachers"
                };
            }

            return new Response()
            {
                Success = true,
                Message = user.FirstName +" "+ user.LastName,
                StatusCode = System.Net.HttpStatusCode.OK
            };
        }
    }
}
