namespace EnglishCenter.Presentation.Models.DTOs
{
    public class ClassMaterialDto
    {
        public string? Title { set; get; }
        public IFormFile? File { set; get; }
        public string? ClassId { set; get; }
        public long? LessonId { set; get; }
    }
}
