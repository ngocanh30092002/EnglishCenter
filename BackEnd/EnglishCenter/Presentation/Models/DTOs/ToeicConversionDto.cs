using System.ComponentModel.DataAnnotations;
using EnglishCenter.Presentation.Global.Enum;

namespace EnglishCenter.Presentation.Models.DTOs
{
    public class ToeicConversionDto
    {
        public int NumberCorrect { set; get; }
        public int EstimatedScore { set; get; }
        public string Section { set; get; } = null!;
    }
}
