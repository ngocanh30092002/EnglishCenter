using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnglishCenter.DataAccess.Entities
{
    [Table("Toeic_Direction")]
    public class ToeicDirection
    {
        [Key]
        public long Id { set; get; }
        public string? Introduce_Listening { set; get; }
        public string? Introduce_Reading { set; get; }
        public string? Image { set; get; }
        public string? Part1 { set; get; }
        public string? Audio1 { set; get; }
        public string? Part2 { set; get; }
        public string? Audio2 { set; get; }
        public string? Part3 { set; get; }
        public string? Audio3 { set; get; }
        public string? Part4 { set; get; }
        public string? Audio4 { set; get; }
        public string? Part5 { set; get; }
        public string? Part6 { set; get; }
        public string? Part7 { set; get; }

        [InverseProperty("ToeicDirection")]
        public virtual ICollection<ToeicExam> ToeicExams { get; set; } = new List<ToeicExam>();
    }
}
