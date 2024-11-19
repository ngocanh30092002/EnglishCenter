namespace EnglishCenter.Presentation.Models.DTOs
{
    public class QuesRcDoubleDto
    {
        public IFormFile? Image1 { set; get; }
        public IFormFile? Image2 { set; get; }
        public string? Time { set; get; }
        public int? Quantity { set; get; }
    }
}
