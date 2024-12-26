using System.ComponentModel.DataAnnotations;

namespace EnglishCenter.Presentation.Models.DTOs
{
    public class RoadMapDto
    {
        public string Name { set; get; } = null!;

        [StringLength(10)]
        public string CourseId { set; get; } = null!;

        public int? Order { set; get; }

        public long? RoadMapId { set; get; }
    }
}
