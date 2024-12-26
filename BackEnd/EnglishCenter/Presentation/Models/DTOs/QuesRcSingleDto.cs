using System.ComponentModel.DataAnnotations;

namespace EnglishCenter.Presentation.Models.DTOs
{
    public class QuesRcSingleDto
    {
        public IFormFile? Image { set; get; }
        public string? Time { set; get; }
        public int? Quantity { set; get; }

        [Range(1, 4)]
        public int? Level { set; get; } = 1;

        public List<SubRcSingleDto>? SubRcSingleDtos { set; get; }
        public string SubRcSingleDtoJson { set; get; }
    }
}
