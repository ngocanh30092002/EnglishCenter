namespace EnglishCenter.Presentation.Models.ResDTOs
{
    public class SubmissionTaskResDto
    {
        public long SubmissionId { set; get; }
        public long LessonId { set; get; }
        public string? Title { set; get; }
        public string? Description { set; get; }
        public string? StartTime { set; get; }
        public string? EndTime { set; get; }
        public LessonResDto? Lesson { set; get; }
    }
}
