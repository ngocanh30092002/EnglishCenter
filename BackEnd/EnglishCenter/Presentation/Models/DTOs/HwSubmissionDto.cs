namespace EnglishCenter.Presentation.Models.DTOs
{
    public class HwSubmissionDto
    {
        public long? HomeworkId { set; get; }
        public long? EnrollId { set; get; }
        public string? Date { set; get; }
        public string? Feedback { set; get; }
        public bool? IsPass { set; get; }
        public List<HwSubRecordDto>? Answers { set; get; }
    }
}
