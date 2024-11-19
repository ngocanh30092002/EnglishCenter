using System.ComponentModel.DataAnnotations;

namespace EnglishCenter.Presentation.Models.DTOs
{
    public class EnrollStatusDto
    {
        [Required]
        public int StatusId { set; get; }

        [Required]
        public string Name { set; get; } = null!;
    }
}
