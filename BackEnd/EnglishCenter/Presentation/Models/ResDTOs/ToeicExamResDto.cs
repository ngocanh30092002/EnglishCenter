namespace EnglishCenter.Presentation.Models.ResDTOs
{
    public class ToeicExamResDto
    {
        public long? ToeicId { set; get; }
        public string? Name { set; get; }
        public int? Code { set; get; }
        public int? Year { set; get; }
        public int? Completed_Num { set; get; }
        public int? Point { set; get; }
        public int? TimeMinutes { set; get; }
        public bool? IsFull { set; get; }
    }
}
