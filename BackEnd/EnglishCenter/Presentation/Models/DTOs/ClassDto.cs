using System.ComponentModel.DataAnnotations;

namespace EnglishCenter.Presentation.Models.DTOs
{
    public class ClassDto
    {
        [Required]
        public string ClassId { set; get; } = null!;

        [Required]
        public string CourseId { set; get; } = null!;

        [Required]
        public string TeacherId { set; get; } = null!;
        public DateOnly? StartDate { set; get; }
        public DateOnly? EndDate { set; get; }

        [Range(0,int.MaxValue)]
        public int? RegisteringNum { set; get; }

        [Range(0, int.MaxValue)]
        public int? RegisteredNum { set; get; }

        [Range(0, int.MaxValue)]
        public int? MaxNum { set; get; }
        public IFormFile? Image { set; get; }
        public string? ImageUrl { set; get; }

    }
}
