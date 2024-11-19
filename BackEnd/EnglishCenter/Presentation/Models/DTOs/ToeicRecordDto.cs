using System.ComponentModel.DataAnnotations;

namespace EnglishCenter.Presentation.Models.DTOs
{
    public class ToeicRecordDto
    {
        [Required]
        public long ProcessId { set; get; }

        [Required]
        public long SubId { set; get; }

        public string? SelectedAnswer { set; get; }

        public bool? IsCorrect { set; get; }
    }
}
