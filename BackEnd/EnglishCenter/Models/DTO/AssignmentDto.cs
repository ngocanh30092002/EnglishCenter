using System.ComponentModel.DataAnnotations;

namespace EnglishCenter.Models.DTO
{
    public class AssignmentDto
    {
        public long? AssignmentId { set; get; }
        public int? NoNum { set; get; }
        public string? Title { set; get; }
        public string? Time { set; get; }
        
        [Required]
        public long ContentId { set; get; }
    }
}
