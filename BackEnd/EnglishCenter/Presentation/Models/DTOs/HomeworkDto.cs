using System.ComponentModel.DataAnnotations;

namespace EnglishCenter.Presentation.Models.DTOs
{
    public class HomeworkDto
    {
        public long LessonId { set; get; }
        public string StartTime { set; get; } = null!;
        public string EndTime { set; get; } = null!;
        public int? LateSubmitDays { set; get; }

        [Required]
        public int Achieved_Percentage { set; get; }

        public string Title { set; get; } = null!;

        public string Time { set; get; } = null!;

        public int? Type { set; get; } = 1;
        public IFormFile Image { set; get; }
    }
}
