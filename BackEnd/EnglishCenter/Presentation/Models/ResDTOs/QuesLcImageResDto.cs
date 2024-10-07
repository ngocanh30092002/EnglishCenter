using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.Presentation.Models.ResDTOs
{
    public class QuesLcImageResDto
    {
        public long? Id { set; get; }
        public string? ImageUrl { set; get; }
        public string? AudioUrl { set;get; }
        public AnswerLcImageDto? AnswerLcImage { set; get;}
    }
}
