using EnglishCenter.DataAccess.Database;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.IRepositories;

namespace EnglishCenter.DataAccess.Repositories.AuthenticationRepositories
{
    public class TeacherRepository : GenericRepository<Teacher> , ITeacherRepository
    {
        public TeacherRepository(EnglishCenterContext context) : base(context)
        {
        }

        public string GetFullName(Teacher teacherModel)
        {
            if (teacherModel == null) return string.Empty;

            var result = teacherModel.FirstName + " " + teacherModel.LastName;
            return result.Trim();
        }

        public async Task<string> GetFullNameAsync(string userId)
        {
            var teacherModel = await context.Teachers.FindAsync(userId);
            if (teacherModel == null) return string.Empty;

            string result = teacherModel.FirstName + " " + teacherModel.LastName;
            return result.Trim();
        }
    }
}
