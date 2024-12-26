using EnglishCenter.DataAccess.Database;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.IRepositories;

namespace EnglishCenter.DataAccess.Repositories.ClassRepositories
{
    public class ClassMaterialRepository : GenericRepository<ClassMaterial>, IClassMaterialRepository
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ClassMaterialRepository(EnglishCenterContext context, IWebHostEnvironment webHostEnvironment) : base(context)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<bool> ChangeClassAsync(ClassMaterial material, string classId)
        {
            if (material == null) return false;

            var classModel = await context.Classes.FindAsync(classId);
            if (classModel == null) return false;

            material.ClassId = classId;

            return true;
        }

        public Task<bool> ChangeFilePathAsync(ClassMaterial material, string newFilePath)
        {
            if (material == null) return Task.FromResult(false);

            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, newFilePath);
            if (!File.Exists(filePath)) return Task.FromResult(false);

            material.FilePath = newFilePath;

            return Task.FromResult(true);
        }

        public async Task<bool> ChangeLessonAsync(ClassMaterial material, long lessonId)
        {
            if (material == null) return false;

            var lessonModel = await context.Lessons.FindAsync(lessonId);
            if (lessonModel == null) return false;

            material.LessonId = lessonId;

            return true;
        }

        public Task<bool> ChangeTitleAsync(ClassMaterial material, string newTitle)
        {
            if (material == null) return Task.FromResult(false);

            material.Title = newTitle;

            return Task.FromResult(true);
        }

        public Task<bool> ChangeUploadByAsync(ClassMaterial material, string uploadBy)
        {
            if (material == null) return Task.FromResult(false);

            material.UploadBy = uploadBy;

            return Task.FromResult(true);
        }
    }
}
