namespace EnglishCenter.Presentation.Models.DTOs
{
    public class RandomPartWithLevelDto
    {
        public long? HomeworkId { set; get; }
        public long? RoadMapExamId { set; get; }
        public int Part { set; get; }
        public int NumNormal { set; get; } = 0;
        public int NumIntermediate { set; get; } = 0;
        public int NumHard { set; get; } = 0;
        public int NumVeryHard { set; get; } = 0;
    }
}
