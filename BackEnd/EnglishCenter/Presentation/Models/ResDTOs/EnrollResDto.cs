using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.Presentation.Models.ResDTOs
{
    public class EnrollResDto
    {
        public long? EnrollId { set; get; }
        public DateOnly? EnrollDate { set; get; }
        public DateTime? UpdateTime { set; get; }
        public UserInfoDto? Student { set; get; }
        public UserBackgroundDto? StudentBackground { set; get; }
        public ClassResDto? Class { set; get; }
        public string? EnrollStatus { set; get; }
        public string? TeacherName { set; get; }
    }
}
