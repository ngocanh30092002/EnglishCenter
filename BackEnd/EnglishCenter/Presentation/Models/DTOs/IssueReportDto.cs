namespace EnglishCenter.Presentation.Models.DTOs
{
    public class IssueReportDto
    {
        public string? UserId { set; get; }
        public string Title { set; get; } = null!;
        public string? Description { set; get; }
        public int Type { set; get; }
    }
}
