using Microsoft.AspNetCore.Mvc;

namespace EnglishCenter.Presentation.Controllers.ExtensionPage
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FilesController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet()]
        public IActionResult DownloadFile(string fileName)
        {
            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, fileName);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            var fileBytes = System.IO.File.ReadAllBytes(filePath);

            return File(fileBytes, "application/octet-stream", fileName);
        }
    }
}
