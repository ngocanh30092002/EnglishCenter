using System.ComponentModel.DataAnnotations;

namespace EnglishCenter.Presentation.Models.DTOs
{
    public class ClassScheduleDto
    {
        [StringLength(10)]
        public string ClassId { set; get; } = null!;

        [Range(0, 6)]
        public int DayOfWeek { set; get; }

        [Range(1, 12)]
        public int StartPeriod { set; get; }

        [Range(1, 12)]
        public int EndPeriod { set; get; }
        public long ClassRoomId { set; get; }
    }
}
