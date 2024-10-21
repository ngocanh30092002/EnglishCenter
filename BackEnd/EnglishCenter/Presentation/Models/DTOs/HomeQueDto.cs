using System.ComponentModel.DataAnnotations;

namespace EnglishCenter.Presentation.Models.DTOs
{
    public class HomeQueDto
    {
        [Required]
        public long QuesId { set; get; }

        [Required]
        public long HomeworkId { set; get; }

        [Required]
        public int Type { set; get; }

        public int? NoNum { set; get; }
    }
}
