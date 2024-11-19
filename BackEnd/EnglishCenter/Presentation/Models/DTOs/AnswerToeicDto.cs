using System.ComponentModel.DataAnnotations;

namespace EnglishCenter.Presentation.Models.DTOs
{
    public class AnswerToeicDto
    {
        public string? AnswerA { set; get; }
        public string? AnswerB { set; get; }
        public string? AnswerC { set; get; }
        public string? AnswerD { set; get; }
        public string? Explanation { set; get; }
        public string? Question { set; get; }

        [StringLength(1)]
        [RegularExpression("^[ABCD]$", ErrorMessage = "Only A, B, C, D are accepted")]
        public string CorrectAnswer { set; get; } = null!;
    }
}
