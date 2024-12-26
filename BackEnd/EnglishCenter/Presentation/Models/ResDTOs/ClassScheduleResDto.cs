using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.Presentation.Models.ResDTOs
{
    public class ClassScheduleResDto
    {
        public long ScheduleId { set; get; }
        public string? ClassId { set; get; }
        public int DayOfWeek { set; get; }
        public string? DayOfWeekStr { set; get; }
        public int StartPeriod { set; get; }
        public string? StartPeriodStr { set; get; }
        public int EndPeriod { set; get; }
        public string? EndPeriodStr { set; get; }
        public ClassRoomDto? ClassRoomInfo { set; get; }
    }
}
