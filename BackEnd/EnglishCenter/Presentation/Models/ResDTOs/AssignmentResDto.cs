namespace EnglishCenter.Presentation.Models.ResDTOs
{
    public class AssignmentResDto
    {
        public long? AssignmentId { set; get; }
        public int? NoNum { set; get; }
        public string? Title { set; get; }
        public string? Time { set; get; }
        public string? ExpectedTime { set; get; }
        public string? Achieved_Percentage { set; get; }
        public string? Status { set; get; }
        public string CourseContentTitle { set; get; }
        public bool CanViewResult { set; get; }
        public List<AssignQueResDto>? AssignQues { set; get; }
    }
}
