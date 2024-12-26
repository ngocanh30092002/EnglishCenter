namespace EnglishCenter.Presentation.Models.ResDTOs
{
    public class AttendanceResDto
    {
        public long AttendId { set; get; }
        public long EnrollId { set; get; }
        public long LessonId { set; get; }
        public bool? IsAttended { set; get; } = false;
        public bool? IsPermitted { set; get; } = false;
        public bool? IsLate { set; get; } = false;
        public bool? IsLeaved { set; get; } = false;
        public UserInfoResDto? UserInfo { set; get; }
    }
}
