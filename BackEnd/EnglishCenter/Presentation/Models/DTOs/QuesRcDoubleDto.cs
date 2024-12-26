using System.ComponentModel.DataAnnotations;
using EnglishCenter.DataAccess.Entities;

namespace EnglishCenter.Presentation.Models.DTOs
{
    public class QuesRcDoubleDto
    {
        public IFormFile? Image1 { set; get; }
        public IFormFile? Image2 { set; get; }
        public string? Time { set; get; }
        public int? Quantity { set; get; }

        [Range(1, 4)]
        public int? Level { set; get; } = 1;

        public List<SubRcDoubleDto>? SubRcDoubleDtos { set; get; }
        public string SubRcDoubleDtoJson { set; get; }
    }
}
