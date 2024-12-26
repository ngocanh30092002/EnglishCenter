namespace EnglishCenter.Presentation.Models.ResDTOs
{
    public class QuesRcSingleResDto
    {
        public long? Id { set; get; }
        public string? ImageUrl { set; get; }
        public TimeOnly? Time { set; get; }
        public int Level { set; get; }
        public List<SubRcSingleResDto>? Questions { set; get; }
    }
}
