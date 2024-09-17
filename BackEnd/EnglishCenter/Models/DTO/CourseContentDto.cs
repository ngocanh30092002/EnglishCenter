using System.ComponentModel.DataAnnotations;

namespace EnglishCenter.Models.DTO
{
    public class CourseContentDto
    {
        public long? ContentId { set; get; }

        [Required]
        public int NoNum { set; get; }

        [Required]
        [StringLength(200)]
        public string Title { set; get; }
        [Required]
        [StringLength(200)]
        public string Content { set; get; }

        [Required]
        [StringLength(10)]
        public string CourseId { set; get; }

        public ICollection<AssignmentDto>? Assignments { get; set; }
    }
}
