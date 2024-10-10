using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.Presentation.Models.ResDTOs
{
    public class QuesLcAudioResDto
    {
        public long? Id { set; get; }
        public string? AudioUrl { set; get; }
        public string? Question { set; get; }
        public string? AnswerA { set; get; }
        public string? AnswerB { set; get; }
        public string? AnswerC { set; get; }
        public TimeOnly? Time { set; get; }
        public AnswerLcAudioDto? AnswerInfo { set; get; }
    }
}
