namespace EnglishCenter.Presentation.Models.ResDTOs
{
    public class QuesToeicResDto
    {
        public long QuesId { set; get; }
        public long ToeicId { set; get; }
        public string? Audio { set; get; }
        public string? Image_1 { set; get; }
        public string? Image_2 { set; get; }
        public string? Image_3 { set; get; }
        public bool IsGroup { set; get; }
        public int Part { set; get; }
        public string? Part_Name { set; get; }
        public List<SubToeicResDto> SubQues { set; get; }
    }
}
