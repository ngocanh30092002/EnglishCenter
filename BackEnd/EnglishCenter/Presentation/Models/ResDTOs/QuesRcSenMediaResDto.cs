using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.Presentation.Models.ResDTOs
{
    public class QuesRcSenMediaResDto
    {
        public long? Id { set; get; }
        public string? Question { set; get; }
        public string? AnswerA { set; get; }
        public string? AnswerB { set; get; }
        public string? AnswerC { set; get; }
        public string? AnswerD { set; get; }
        public TimeOnly? Time { set; get; }
        public AnswerRcSenMediaDto? AnswerInfo { set; get; }
    }
}
