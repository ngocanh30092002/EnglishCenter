using System.ComponentModel.DataAnnotations;

namespace EnglishCenter.Presentation.Models.DTOs
{
    public class AnswerLcAudioDto
    {
        public string AnswerA { set; get; } = null!;
        public string AnswerB { set; get; } = null!;
        public string AnswerC { set; get; } = null!;

        [StringLength(1)]
        public string CorrectAnswer { set; get; } = null!;
    }
}
