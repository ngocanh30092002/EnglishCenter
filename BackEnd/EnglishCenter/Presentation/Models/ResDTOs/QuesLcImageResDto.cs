using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.Presentation.Models.ResDTOs
{
    public class QuesLcImageResDto
    {
        public long? Id { set; get; }
        public string? ImageUrl { set; get; }
        public string? AudioUrl { set; get; }
        public int Level { set; get; }
        public TimeOnly? Time { set; get; }
        public AnswerLcImageDto? AnswerInfo { set; get; }
    }
}
