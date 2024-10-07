namespace EnglishCenter.Presentation.Models.DTOs
{
    public class QuesLcAudioDto
    {
        public IFormFile? Audio { set; get; }
        public long? AnswerId { set; get; }
        public AnswerLcAudioDto? Answer { set;get; }
    }
}
