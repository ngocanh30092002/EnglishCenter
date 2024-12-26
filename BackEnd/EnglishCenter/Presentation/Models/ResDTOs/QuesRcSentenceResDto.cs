using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.Presentation.Models.ResDTOs
{
    public class QuesRcSentenceResDto
    {
        public long? Id { set; get; }
        public string? Question { set; get; }
        public string? AnswerA { set; get; }
        public string? AnswerB { set; get; }
        public string? AnswerC { set; get; }
        public string? AnswerD { set; get; }
        public TimeOnly? Time { set; get; }
        public int Level { set; get; }
        public AnswerRcSentenceDto? AnswerInfo { set; get; }
    }
}
