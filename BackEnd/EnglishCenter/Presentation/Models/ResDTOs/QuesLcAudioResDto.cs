using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.Presentation.Models.ResDTOs
{
    public class QuesLcAudioResDto
    {
        public long? Id { set; get; }
        public string? AudioUrl { set; get; }
        public AnswerLcAudioDto? AnswerInfo { set; get; }
    }
}
