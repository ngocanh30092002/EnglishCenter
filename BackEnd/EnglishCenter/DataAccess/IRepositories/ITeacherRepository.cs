using EnglishCenter.DataAccess.Entities;

namespace EnglishCenter.DataAccess.IRepositories
{
    public interface ITeacherRepository
    {
        public string GetFullName(Student teacher);
    }
}
