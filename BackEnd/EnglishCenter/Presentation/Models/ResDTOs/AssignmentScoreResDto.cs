namespace EnglishCenter.Presentation.Models.ResDTOs
{
    public class AssignmentScoreResDto
    {
        public int Correct { set; get; } = 0;
        public int InCorrect { set; get; } = 0;
        public int Total { set; get; } = 0;
        public double? Achieve_Percentage { set; get; } = 0;
        public double? Current_Percentage { set; get; } = 0;
        public bool IsPass = false;
    }
}
