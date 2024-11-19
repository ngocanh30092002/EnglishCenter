using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.Presentation.Models.ResDTOs
{
    public class CourseContentResDto
    {
        public long ContentId { set; get; }
        public int NoNum { set; get; }
        public string? Title { set; get; }
        public string? Content { set; get; }
        public string? CourseId { set; get; }
        public int Type { set; get; } = 1;
        public ICollection<AssignmentResDto>? Assignments { get; set; }
        public ExaminationDto? Examination { set; get; }
    }
}
