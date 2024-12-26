namespace EnglishCenter.Presentation.Models.ResDTOs
{
    public class ClassMaterialResDto
    {
        public long ClassMaterialId { set; get; }
        public string? Title { set; get; }
        public string? FilePath { set; get; }
        public string? UploadAt { set; get; }
        public string? UploadBy { set; get; }
        public string? ClassId { set; get; }
        public LessonResDto? LessonInfo { set; get; }
        public string? FileSize { set; get; }
        public string? Type { set; get; }
        public string? LessonDate { set; get; }
    }
}
