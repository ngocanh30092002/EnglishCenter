namespace EnglishCenter.Presentation.Models.DTOs
{
    public class SubmissionFileDto
    {
        public IFormFile? File { set; get; }
        public string? LinkUrl { set; get; }
        public long SubmissionTaskId { set; get; }
        public long EnrollId { set; get; }
        public List<IFormFile>? Files { set; get; }
    }
}
