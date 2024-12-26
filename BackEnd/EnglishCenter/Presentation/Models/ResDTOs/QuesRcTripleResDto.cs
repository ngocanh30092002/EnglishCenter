namespace EnglishCenter.Presentation.Models.ResDTOs
{
    public class QuesRcTripleResDto
    {
        public long? Id { set; get; }
        public string? ImageUrl_1 { set; get; }
        public string? ImageUrl_2 { set; get; }
        public string? ImageUrl_3 { set; get; }
        public int Level { set; get; }
        public TimeOnly? Time { set; get; }
        public List<SubRcTripleResDto>? Questions { set; get; }
    }
}
