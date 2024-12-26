namespace EnglishCenter.Presentation.Models.ResDTOs
{
    public class RoadMapExamResDto
    {
        public long Id { set; get; }
        public string Name { set; get; }
        public long DirectionId { set; get; }
        public int Point { set; get; }
        public double Time_Minutes { set; get; }
        public int Completed_Num { set; get; }
        public long RoadMapId { set; get; }
    }
}
