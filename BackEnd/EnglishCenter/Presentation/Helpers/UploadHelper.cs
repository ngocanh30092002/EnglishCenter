using System.IO;
using System.Linq;

namespace EnglishCenter.Presentation.Helpers
{
    public static class UploadHelper
    {
        public static async Task<string> UploadFileAsync(IFormFile file, string folderPath, string fileName)
        {
            if (file == null || file.Length == 0)
            {
                return "Can't read any content in the file";
            }

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var filePath = Path.Combine(folderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
                stream.Position = 0;
            }

            return null;
        }

        public static Task<bool> IsImageAsync(IFormFile imageFile)
        {
            var allowedMimeTypes = new[] { "image/jpeg", "image/png", "image/gif", "image/svg+xml" };
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".svg" };
            var fileExtension = Path.GetExtension(imageFile.FileName).ToLower();

            if (!allowedMimeTypes.Contains(imageFile.ContentType.ToLower()) || !allowedExtensions.Contains(fileExtension))
            {
                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }

        public static Task<bool> IsAudioAsync(IFormFile audioFile)
        {
            var allowedMimeTypes = new[] { "audio/mpeg", "audio/wav", "audio/ogg" };
            var allowedExtensions = new[] { ".mp3", ".mp4", ".wav", ".ogg" };
            var fileExtension = Path.GetExtension(audioFile.FileName).ToLower();

            if (!allowedMimeTypes.Contains(audioFile.ContentType.ToLower()) || !allowedExtensions.Contains(fileExtension))
            {
                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }
    }
}
