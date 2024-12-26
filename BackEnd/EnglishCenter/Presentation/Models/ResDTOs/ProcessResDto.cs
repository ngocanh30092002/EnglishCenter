using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.Presentation.Models.ResDTOs
{
    public class ProcessResDto
    {
        public long ProcessID { set; get; }
        public UserInfoResDto UserInfo { set; get; }
        public string Title { set; get; }
        public string Status { set; get; }
        public string Time { set; get; }
        public int CorrectNumber { set; get; }
        public int TotalNumber { set; get; }
        public double CurrentRate { set; get; }
        public AssignmentResDto? AssignmentInfo { set; get; }
        public ExaminationDto? ExamInfo { set; get; }
    }
}
