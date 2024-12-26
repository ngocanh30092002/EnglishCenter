namespace EnglishCenter.Presentation.Models.DTOs
{
    public class UserAttemptDto
    {
        public string? UserId { set; get; }
        public long? ToeicId { set; get; }
        public long? RoadMapExamId { set; get; }
        public int? Listening_Score { set; get; }
        public int? Reading_Score { set; get; }
        public List<AttemptRecordDto>? PracticeRecords { set; get; }
    }
}
