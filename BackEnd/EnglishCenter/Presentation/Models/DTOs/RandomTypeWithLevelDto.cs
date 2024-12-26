namespace EnglishCenter.Presentation.Models.DTOs
{
    public class RandomTypeWithLevelDto
    {
        public int Type { set; get; }
        public int NumNormal { set; get; } = 0;
        public int NumIntermediate { set; get; } = 0;
        public int NumHard { set; get; } = 0;
        public int NumVeryHard { set; get; } = 0;
    }
}
