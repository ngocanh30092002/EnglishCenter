namespace EnglishCenter.Presentation.Models.ResDTOs
{
    public class LearningProcessResDto
    {
        public long ProcessId { set; get; }
        public long EnrollId { set; get; }
        public string? Status { set; get; }
        public string? StartTime { set; get; }
        public string? EndTime { set; get; }
        public long? AssignmentId { set; get; }
        public long? ExamId { set; get; }
        public string? Result { set; get; }
    }
}
