namespace EnglishCenter.Presentation.Models.ResDTOs
{
    public class IssueReportResDto
    {
        public long IssueId { set; get; }
        public string? Title { set; get; }
        public string? Description { set; get; }
        public int Type { set; get; }
        public string? TypeName { set; get; }
        public int Status { set; get; }
        public string? StatusName { set; get; }
        public string? UserName { set; get; }
        public string? Image { set; get; }
        public string? Email { set; get; }
        public string? CreatedAt { set; get; }
        public List<string>? Roles { set; get; }

        public List<IssueResponseResDto>? Responses { set; get; }
    }
}
