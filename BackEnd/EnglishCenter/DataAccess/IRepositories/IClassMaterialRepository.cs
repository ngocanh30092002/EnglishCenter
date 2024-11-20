using EnglishCenter.DataAccess.Entities;

namespace EnglishCenter.DataAccess.IRepositories
{
    public interface IClassMaterialRepository : IGenericRepository<ClassMaterial>
    {
        public Task<bool> ChangeTitleAsync(ClassMaterial material, string newTitle);
        public Task<bool> ChangeFilePathAsync(ClassMaterial material, string newFilePath);
        public Task<bool> ChangeUploadByAsync(ClassMaterial material, string uploadBy);
        public Task<bool> ChangeClassAsync(ClassMaterial material, string classId);
        public Task<bool> ChangeLessonAsync(ClassMaterial material, long lessonId);
    }
}
