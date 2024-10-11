using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.Presentation.Models.ResDTOs
{
    public class SubRcDoubleResDto
    {
        public long QuesId { set; get; }
        public string? Question { set; get; }
        public string? AnswerA { set; get; }
        public string? AnswerB { set; get; }
        public string? AnswerC { set; get; }
        public string? AnswerD { set; get; }
        public AnswerRcDoubleDto? AnswerInfo { set; get; }
    }
}
