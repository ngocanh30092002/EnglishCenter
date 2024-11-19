using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.Presentation.Models.ResDTOs
{
    public class SubToeicResDto
    {
        public long? SubId { set; get; }
        public int? QuesNo { set; get; }
        public string? Question { set; get; }
        public string? AnswerA { set; get; }
        public string? AnswerB { set; get; }
        public string? AnswerC { set; get; }
        public string? AnswerD { set; get; }
        public AnswerToeicDto? AnswerInfo { set; get; }
    }
}
