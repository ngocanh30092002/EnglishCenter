namespace EnglishCenter.Presentation.Models.ResDTOs
{
    public class UserWordResDto
    {
        public long UserWordId { set; get; }
        public string? UserId { set; get; }
        public string? Word { set; get; }
        public string? Translation { set; get; }
        public string? Phonetic { set; get; }
        public string? Image { set; get; }
        public string? Tag { set; get; }
        public string? Type { set; get; }
        public bool IsFavorite { set; get; }
        public string? UpdateDate { set; get; }
    }
}
