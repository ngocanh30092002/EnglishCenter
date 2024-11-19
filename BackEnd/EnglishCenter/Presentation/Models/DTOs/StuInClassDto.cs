using System.ComponentModel.DataAnnotations;

namespace EnglishCenter.Presentation.Models.DTOs
{
    public class StuInClassDto
    {
        public long? StuInClassId { get; set; }

        [StringLength(100)]
        public string? UserId { get; set; }

        [StringLength(10)]
        public string? ClassId { get; set; }

        public long? ScoreHisId { get; set; }

        public int? Status { set; get; }
    }
}
