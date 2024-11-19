namespace EnglishCenter.Presentation.Models.DTOs
{
    public class ScoreHistoryDto
    {
        public long ScoreHisId { set; get; }
        public int? EntrancePoint { get; set; }
        public int? MidtermPoint { get; set; }
        public int? FinalPoint { get; set; }
    }
}
