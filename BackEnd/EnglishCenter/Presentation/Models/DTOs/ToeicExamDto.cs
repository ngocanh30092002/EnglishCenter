using System.ComponentModel.DataAnnotations;

namespace EnglishCenter.Presentation.Models.DTOs
{
    public class ToeicExamDto
    {
        [Required]
        public string Name { set; get; } = null!;

        [Required]
        public int Code { set; get; }

        [Required]
        public int Year { set; get; }

        public int? Completed_Num { set; get; } = 0;
        public int? Point { set; get; } = 990;
        public int? TimeMinutes { set; get; } = 120;
    }
}