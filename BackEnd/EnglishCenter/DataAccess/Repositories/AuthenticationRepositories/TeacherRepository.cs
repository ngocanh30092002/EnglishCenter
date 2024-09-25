using EnglishCenter.DataAccess.Database;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.IRepositories;

namespace EnglishCenter.DataAccess.Repositories.AuthenticationRepositories
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

        public string GetFullName(Student teacher)
        {
            if (teacher == null) return string.Empty;

            var result = teacher.FirstName + " " + teacher.LastName;
            return result.Trim();
        }
    }
}
