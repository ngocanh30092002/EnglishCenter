using System.ComponentModel.DataAnnotations;

namespace EnglishCenter.Presentation.Models.DTOs
{
    public class QuesLcImageDto
    {
        public IFormFile? Image { set; get; }
        public IFormFile? Audio { set; get; }
        public long? AnswerId { set; get; }

        [Range(1, 4)]
        public int? Level { set; get; } = 1;
        public AnswerLcImageDto? Answer { set; get; } = null;
    }
}
