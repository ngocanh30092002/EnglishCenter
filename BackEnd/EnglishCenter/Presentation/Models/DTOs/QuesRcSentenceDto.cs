namespace EnglishCenter.Presentation.Models.DTOs
{
    public class QuesRcSentenceDto
    {
        public string Question { set; get; } = null!;
        public string AnswerA { set; get; } = null!;
        public string AnswerB { set; get; } = null!;
        public string AnswerC { set; get; } = null!;
        public string AnswerD { set; get; } = null!;
        public long? AnswerId { set; get; }
        public AnswerRcSentenceDto? Answer { set; get;}
    }
}
