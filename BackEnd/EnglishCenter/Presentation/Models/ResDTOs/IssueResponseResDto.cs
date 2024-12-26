namespace EnglishCenter.Presentation.Models.ResDTOs
{
    public class IssueResponseResDto
    {
        public long IssueResId { set; get; }
        public long IssueId { set; get; }
        public string? UserId { set; get; }
        public string? Image { set; get; }
        public string? Message { set; get; }
        public string? CreatedAt { set; get; }
    }
}
