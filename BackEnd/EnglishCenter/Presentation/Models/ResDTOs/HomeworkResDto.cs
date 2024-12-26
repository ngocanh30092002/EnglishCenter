namespace EnglishCenter.Presentation.Models.ResDTOs
{
    public class HomeworkResDto
    {
        public long? HomeworkId { set; get; }
        public long? LessonId { set; get; }
        public string? StartTime { set; get; }
        public string? EndTime { set; get; }
        public int? LateSubmitDays { set; get; }
        public int? Achieved_Percentage { set; get; }
        public string? ExpectedTime { set; get; }
        public string? Time { set; get; }
        public string? Title { set; get; }
        public int Status { set; get; }
        public string Image { set; get; }
        public int Type { set; get; }
    }
}
