﻿
namespace EnglishCenter.Helpers
{
    public static class UploadHelper
    {
        public static async Task<string> UploadFileAsync(IFormFile file, string folderPath, string fileName)
        {
            if (file == null || file.Length == 0)
            {
                return "Can't read any content in the file";
            }

            if(!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var filePath = Path.Combine(folderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return null;
        }
    }
}
