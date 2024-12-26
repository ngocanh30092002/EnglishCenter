namespace EnglishCenter.Presentation.Models.DTOs
{
    public class IssueResponseDto
    {
        public string? UserId { set; get; }
        public long IssueId { set; get; }
        public string Message { set; get; } = null!;
    }
}
