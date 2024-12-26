using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnglishCenter.DataAccess.Entities
{
    [Table("Random_Ques_Toeic")]
    public class RandomQuesToeic
    {
        [Key]
        public long Id { set; get; }
        public long? RoadMapExamId { set; get; }

        [ForeignKey("RoadMapExamId")]
        [InverseProperty("RandomQues")]
        public virtual RoadMapExam? RoadMapExam { get; set; }

        public long? HomeworkId { set; get; }

        [ForeignKey("HomeworkId")]
        [InverseProperty("RandomQues")]
        public virtual Homework? Homework { get; set; }
        public long QuesToeicId { set; get; }

        [ForeignKey("QuesToeicId")]
        [InverseProperty("RandomQues")]
        public virtual QuesToeic QuesToeic { set; get; } = null!;
    }
}
