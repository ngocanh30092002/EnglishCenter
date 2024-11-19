using System.ComponentModel.DataAnnotations;

namespace EnglishCenter.Presentation.Models.DTOs
{
    public class HomeworkDto
    {
        public string ClassId { set; get; } = null!;
        public string StartTime { set; get; } = null!;
        public string EndTime { set; get; } = null!;
        public int? LateSubmitDays { set; get; }
        
        [Required]
        public int Achieved_Percentage { set; get; }
        
        public string Title { set; get; } = null!;

        public string Time { set; get; } = null!;
    }
}
