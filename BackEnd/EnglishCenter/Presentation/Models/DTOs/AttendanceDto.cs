namespace EnglishCenter.Presentation.Models.DTOs
{
    public class AttendanceDto
    {
        public long EnrollId { set; get; }
        public long LessonId { set; get; }
        public bool? IsAttended { set; get; } = false;
        public bool? IsPermitted { set; get; } = false;
        public bool? IsLate { set; get; } = false;
        public bool? IsLeaved { set; get; } = false;
    }
}
