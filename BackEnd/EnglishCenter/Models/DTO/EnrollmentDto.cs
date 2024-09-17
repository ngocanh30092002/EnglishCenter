using System.ComponentModel.DataAnnotations;

namespace EnglishCenter.Models.DTO
{
    public class EnrollmentDto
    {
        public string? EnrollId { set; get; }
        public string? UserId { set; get; }

        [Required]
        public string ClassId { set; get; } = null!;

        public DateOnly? EnrollDate { set; get; }
        
        public int? StatusId { set; get; }
    }
}
