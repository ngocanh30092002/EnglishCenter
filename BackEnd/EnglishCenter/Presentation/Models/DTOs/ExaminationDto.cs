namespace EnglishCenter.Presentation.Models.DTOs
{
    public class ExaminationDto
    {
        public long? ExamId { set; get; }
        public long ContentId { set; get; }
        public long ToeicId { set; get; }
        public string? Title { set; get; }
        public string? Time { set; get; }
        public string? Status { set; get; }
        public string? Description { set; get; }
    }
}
