namespace EnglishCenter.Presentation.Models.DTOs
{
    public class UserWordDto
    {
        public string? UserId { set; get; }
        public string Word { set; get; } = null!;
        public string Translation { set; get; } = null!;
        public string Phonetic { set; get; } = null!;
        public IFormFile? Image { set; get; }
        public int Type { set; get; }
        public string Tag { set; get; } = null!;
    }

}
