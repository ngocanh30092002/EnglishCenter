using System.ComponentModel.DataAnnotations;

namespace EnglishCenter.Presentation.Models.DTOs
{
    public class QuesToeicDto
    {
        [Required]
        public long ToeicId { set; get; }
        public IFormFile? Audio { set; get; }
        public IFormFile? Image_1 { set; get; }
        public IFormFile? Image_2 { set; get; }
        public IFormFile? Image_3 { set; get; }
        public bool IsGroup = false;
        public int? NoNum;
        [Required]
        public int Part { set; get; }

        public List<SubToeicDto>? SubToeicDtos { get; set; }

        public string? SubToeicDtoJson { set; get; }
    }
}
