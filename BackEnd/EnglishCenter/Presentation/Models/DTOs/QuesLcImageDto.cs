namespace EnglishCenter.Presentation.Models.DTOs
{
    public class QuesLcImageDto
    {
        public IFormFile? Image { set; get; }
        public IFormFile? Audio { set; get; }
        public long? AnswerId { set; get; }
        public AnswerLcImageDto? Answer { set; get; } = null;
    }
}
