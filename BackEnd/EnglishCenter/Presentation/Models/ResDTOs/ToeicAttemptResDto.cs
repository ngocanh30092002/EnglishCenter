namespace EnglishCenter.Presentation.Models.ResDTOs
{
    public class ToeicAttemptResDto
    {
        public long AttemptId { set;get; }
        public string UserId { set; get; }
        public long ToeicId { set; get; }
        public ToeicExamResDto? ToeicExam { set; get; }
        public int Listening_Score { set; get; }
        public int Reading_Score { set; get; }
        public DateTime Date { set; get; }
    }
}
