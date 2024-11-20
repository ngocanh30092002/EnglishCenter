using System.ComponentModel.DataAnnotations;

namespace EnglishCenter.Presentation.Models.DTOs
{
    public class LessonDto
    {
        [StringLength(10)]
        public string ClassId { set; get; } = null!;

        public string Date { set; get; } = null!;

        [Range(1, 12)]
        public int StartPeriod { set; get; }

        [Range(1, 12)]
        public int EndPeriod { set; get; }

        public long ClassRoomId { set; get; }

        public string? Topic { set; get; }
    }
}
