namespace EnglishCenter.Presentation.Models.DTOs
{
    public class QuesRcTripleDto
    {
        public IFormFile? Image1 { set; get; }
        public IFormFile? Image2 { set; get; }
        public IFormFile? Image3 { set; get; }
        public string? Time { set; get; }
        public int? Quantity { set; get; }
    }
}
