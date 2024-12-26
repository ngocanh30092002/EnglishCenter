namespace EnglishCenter.Presentation.Models.DTOs
{
    public class RoadMapExamDto
    {
        public string Name { set; get; } = null!;
        public int? Point { set; get; } = 0;
        public double TimeMinutes { set; get; }
        public long RoadMapId { set; get; }
    }
}
