namespace EnglishCenter.Presentation.Models.DTOs
{
    public class SubRcTripleDto
    {
        public long PreQuesId { set; get; }
        public string Question { set; get; } = null!;
        public string AnswerA { set; get; } = null!;
        public string AnswerB { set; get; } = null!;
        public string AnswerC { set; get; } = null!;
        public string AnswerD { set; get; } = null!;
        public int? NoNum { set; get; }
        public long? AnswerId { set; get; }
        public AnswerRcTripleDto? Answer { set; get; }
    }
}
