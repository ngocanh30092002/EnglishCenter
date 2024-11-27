namespace EnglishCenter.Presentation.Models.ResDTOs
{
    public class HwSubmissionResDto
    {
        public long SubmissionId { set; get; }
        public HomeworkResDto? Homework { set; get; }
        public string? Date { set; get; }
        public string? Status { set; get; }
        public string? FeedBack { set; get; }
        public bool IsPass { set; get; }
        public HomeworkScoreResDto? Score { set; get; }
    }
}
