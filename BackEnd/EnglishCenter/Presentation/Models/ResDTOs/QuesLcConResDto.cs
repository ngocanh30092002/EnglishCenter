namespace EnglishCenter.Presentation.Models.ResDTOs
{
    public class QuesLcConResDto
    {
        public long? Id { set; get; }
        public string? ImageUrl { set; get; }
        public string? AudioUrl { set; get; }
        public TimeOnly? Time { set; get;}
        public List<SubLcConResDto>? Questions { set; get;}
    }
}
