using System.ComponentModel.DataAnnotations;

namespace EnglishCenter.Presentation.Models.DTOs
{
    public class AnswerRcSentenceDto
    {
        public string AnswerA { set; get; } = null!;
        public string AnswerB { set; get; } = null!;
        public string AnswerC { set; get; } = null!;
        public string AnswerD { set; get; } = null!;
        public string? Explanation { set; get; }
        public string? Question { set; get; }

        [StringLength(1)]
        [RegularExpression("^[ABCD]$", ErrorMessage = "Only A, B, C, D are accepted")]
        public string CorrectAnswer { set; get; } = null!;

    }
}
