namespace EnglishCenter.Presentation.Models.DTOs
{
    public class QuesRcSenMediaDto
    {
        public string Question { set; get; } = null!;
        public IFormFile Image { set; get; } = null!;
        public IFormFile? Audio { set; get; }
        public string AnswerA { set; get; } = null!;
        public string AnswerB { set; get; } = null!;
        public string AnswerC { set; get; } = null!;
        public string AnswerD { set; get; } = null!;
        public string Time { set; get; } = null!;
        public long? AnswerId { set; get; }
        public AnswerRcSenMediaDto? Answer { set; get; }
    }
}
