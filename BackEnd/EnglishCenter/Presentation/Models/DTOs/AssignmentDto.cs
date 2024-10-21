using System.ComponentModel.DataAnnotations;

namespace EnglishCenter.Presentation.Models.DTOs
{
    public class AssignmentDto
    {
        [Required]
        public long ContentId { set; get; }
        public string? Title { set; get; }
        public string? Time { set; get; }
        public int? NoNum { set; get; }

        [Required]
        public int Achieved_Percentage { set; get; }
    }
}
