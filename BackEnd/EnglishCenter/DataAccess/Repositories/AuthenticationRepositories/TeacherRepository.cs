using EnglishCenter.DataAccess.Database;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.IRepositories;
using EnglishCenter.Presentation.Helpers;

namespace EnglishCenter.DataAccess.Repositories.AuthenticationRepositories
{
    public class TeacherRepository : GenericRepository<Teacher>, ITeacherRepository
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public TeacherRepository(EnglishCenterContext context, IWebHostEnvironment webHostEnvironment) : base(context)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<bool> ChangeBackgroundImageAsync(IFormFile file, Teacher teacher)
        {
            if (teacher == null) return false;

            var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "users", "backgrounds");
            var fileName = $"{DateTime.Now.Ticks}_{file.FileName}";
            var result = await UploadHelper.UploadFileAsync(file, uploadFolder, fileName);

            if (!string.IsNullOrEmpty(result)) return false;

            var wwwRootPath = _webHostEnvironment.WebRootPath;
            var previousImage = Path.Combine(wwwRootPath, teacher.BackgroundImage ?? "");

            if (File.Exists(previousImage))
            {
                File.Delete(previousImage);
            }

            teacher.BackgroundImage = Path.Combine("users", "backgrounds", fileName);
            return true;
        }

        public async Task<bool> ChangeTeacherImageAsync(IFormFile file, Teacher teacher)
        {
            if (teacher == null) return false;

            var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "users", "profiles");
            var fileName = $"{DateTime.Now.Ticks}_{file.FileName}";
            var result = await UploadHelper.UploadFileAsync(file, uploadFolder, fileName);

            if (!string.IsNullOrEmpty(result)) return false;

            var wwwRootPath = _webHostEnvironment.WebRootPath;
            var previousImage = Path.Combine(wwwRootPath, teacher.Image ?? "");

            if (File.Exists(previousImage))
            {
                File.Delete(previousImage);
            }

            teacher.Image = Path.Combine("users", "profiles", fileName);
            return true;
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
