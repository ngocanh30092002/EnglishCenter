using EnglishCenter.DataAccess.Entities;

namespace EnglishCenter.Presentation.Models.ResDTOs
{
    public class ClassResDto
    {
        public string? ClassId { set; get; }
        public string? CourseId { set; get; }
        public TeacherResDto? Teacher { set; get; }
        public DateOnly? StartDate { set; get; }
        public DateOnly? EndDate { set; get; }
        public int? RegisteringNum { set; get; }
        public int? RegisteredNum { set; get; }
        public int? MaxNum { set; get; }
        public string? ImageUrl { set; get; }
        public string? Status { set; get; }
        public string? Description { set; get; }
    }
}
