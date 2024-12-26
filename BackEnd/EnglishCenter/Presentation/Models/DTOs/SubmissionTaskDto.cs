namespace EnglishCenter.Presentation.Models.DTOs
{
    public class SubmissionTaskDto
    {
        public long LessonId { set; get; }
        public string Title { set; get; } = null!;
        public string? Description { set; get; }
        public string StartTime { set; get; } = null!;
        public string EndTime { set; get; } = null!;
    }
}
