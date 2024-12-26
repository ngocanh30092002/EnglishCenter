using System.ComponentModel.DataAnnotations;

namespace EnglishCenter.Presentation.Models.DTOs
{
    public class QuesLcConDto
    {
        public IFormFile? Image { set; get; }
        public IFormFile? Audio { set; get; }
        public int? Quantity { set; get; }

        [Range(1, 4)]
        public int? Level { set; get; } = 1;

        public List<SubLcConDto>? SubLcCons { set; get; }
        public string SubLcConsJson { set; get; }
    }
}
