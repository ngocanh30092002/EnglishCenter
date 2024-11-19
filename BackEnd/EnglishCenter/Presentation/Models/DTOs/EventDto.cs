using System.ComponentModel.DataAnnotations;

namespace EnglishCenter.Presentation.Models.DTOs
{
    public class EventDto
    {
        public long? ScheduleId { set; get; }

        [Required]
        [StringLength(300)]

        public string Title { set; get; } = null!;

        [Required]
        [RegularExpression(@"^(0[1-9]|1[0-2]):[0-5][0-9]\s(AM|PM)$", ErrorMessage = "StartTime must be in the format")]
        public string StartTime { set; get; } = null!;

        [Required]
        [RegularExpression(@"^(0[1-9]|1[0-2]):[0-5][0-9]\s(AM|PM)$", ErrorMessage = "EndTime must be in the format")]
        public string EndTime { set; get; } = null!;

        [Required]
        public DateOnly Date { set; get; }
    }
}
