namespace EnglishCenter.Presentation.Models.ResDTOs
{
    public class QuesRcDoubleResDto
    {
        public long? Id { set; get; }
        public string? ImageUrl_1 { set; get; }
        public string? ImageUrl_2 { set; get; }
        public TimeOnly? Time { set; get; }
        public List<SubRcDoubleResDto>? Questions { set; get; }
    }
}
