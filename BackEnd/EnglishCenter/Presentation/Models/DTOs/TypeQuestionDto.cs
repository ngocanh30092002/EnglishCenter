namespace EnglishCenter.Presentation.Models.DTOs
{
    public class TypeQuestionDto
    {
        public int Type { set; get; }
        public List<int> QueIds { set; get; } = new List<int>();
    }
}
