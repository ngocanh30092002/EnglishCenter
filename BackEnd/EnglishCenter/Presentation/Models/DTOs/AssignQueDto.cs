using System.ComponentModel.DataAnnotations;

namespace EnglishCenter.Presentation.Models.DTOs
{
    public class AssignQueDto
    {
        [Required]
        public long QuesId { set; get; }
        
        [Required]
        public long AssignmentId { set; get; }

        [Required]
        public int Type { set; get; }
    }
}
