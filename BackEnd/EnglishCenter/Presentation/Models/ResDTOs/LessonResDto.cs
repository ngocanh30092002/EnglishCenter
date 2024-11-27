using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.Presentation.Models.ResDTOs
{
    public class LessonResDto
    {
        public long LessonId { set; get; }
        public string ClassId { set; get; }
        public string DayOfWeek { set; get; }
        public string Date { set; get; }
        public int StartPeriod { set; get; }
        public string StartPeriodTime { set; get; }
        public int EndPeriod { set; get; }
        public string EndPeriodTime { set; get; }
        public string Topic { set; get; }
        public ClassRoomDto ClassRoom { set; get; }
    }
}
