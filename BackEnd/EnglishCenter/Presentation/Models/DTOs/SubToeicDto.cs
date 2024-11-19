namespace EnglishCenter.Presentation.Models.DTOs
{
    public class SubToeicDto
    {
        public long? QuesId { set; get; }
        public int? QuesNo { set; get; }
        public string? Question { set; get; }
        public string? AnswerA { set; get; }
        public string? AnswerB { set; get; }
        public string? AnswerC { set; get; }
        public string? AnswerD { set; get; }
        public long? AnswerId { set; get; }
        public AnswerToeicDto? Answer { set; get; }
    }
}
