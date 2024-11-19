namespace EnglishCenter.Presentation.Models.DTOs
{
    public class ToeicDirectionDto
    {
        public string Introduce_Listening { set; get; } = null!;
        public string Introduce_Reading { set; get; } = null!;
        public IFormFile? imageFile { set; get; }
        public string Part_1 { set; get; } = null!;
        public IFormFile? Audio1 { set; get; } = null!;
        public string Part_2 { set; get; } = null!;
        public IFormFile? Audio2 { set; get; } = null!;
        public string Part_3 { set; get; } = null!;
        public IFormFile? Audio3 { set; get; } = null!;
        public string Part_4 { set; get; } = null!;
        public IFormFile? Audio4 { set; get; } = null!;
        public string Part_5 { set; get; } = null!;
        public string Part_6 { set; get; } = null!;
        public string Part_7 { set; get; } = null!;

    }
}
