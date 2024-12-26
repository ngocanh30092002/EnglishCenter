using System.ComponentModel.DataAnnotations;

namespace EnglishCenter.Presentation.Models.DTOs
{
    public class QuesLcAudioDto
    {
        public IFormFile? Audio { set; get; }
        public string Question { set; get; } = null!;
        public string AnswerA { set; get; } = null!;
        public string AnswerB { set; get; } = null!;
        public string AnswerC { set; get; } = null!;
        public long? AnswerId { set; get; }
        public AnswerLcAudioDto? Answer { set; get; }

        [Range(1, 4)]
        public int? Level { set; get; } = 1;
    }
}
