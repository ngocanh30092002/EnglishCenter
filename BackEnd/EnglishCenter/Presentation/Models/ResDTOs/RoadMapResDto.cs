namespace EnglishCenter.Presentation.Models.ResDTOs
{
    public class RoadMapResDto
    {
        public long RoadMapId { set; get; }
        public string? RoadMapName { set; get; }
        public string? CourseId { set; get; }
        public List<RoadMapExamResDto>? RoadMapExams { set; get; }
    }
}
