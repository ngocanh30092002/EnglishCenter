using System.ComponentModel.DataAnnotations;

namespace EnglishCenter.Presentation.Models.DTOs
{
    public class EnrollmentDto
    {
        public string? EnrollId { set; get; }
        public string? UserId { set; get; }

        [Required]
        public string ClassId { set; get; } = null!;

        public DateOnly? EnrollDate { set; get; }

        public int? StatusId { set; get; }
        public long? ScoreHisId { set; get; }
        public DateTime? UpdateTime { set; get; }
    }
}
