using System.ComponentModel.DataAnnotations;
using EnglishCenter.Presentation.Models.ResDTOs;

namespace EnglishCenter.Presentation.Models.DTOs
{
    public class CourseContentDto
    {
        public long? ContentId { set; get; }

        public int? NoNum { set; get; }

        [Required]
        [StringLength(200)]
        public string Title { set; get; }
        [Required]
        [StringLength(200)]
        public string Content { set; get; }

        [Required]
        [StringLength(10)]
        public string CourseId { set; get; }

        public ICollection<AssignmentResDto>? Assignments { get; set; }
    }
}
