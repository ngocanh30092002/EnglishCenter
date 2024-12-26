using System.ComponentModel.DataAnnotations;

namespace EnglishCenter.Presentation.Models.DTOs
{
    public class RandomPartDto
    {
        public long? HomeworkId { set; get; }
        public long? RoadMapExamId { set; get; }
        public int Part { set; get; }

        [Range(1, 4)]
        public int Level { set; get; } = 1;
        public int Number { set; get; }
    }
}
