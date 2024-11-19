using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnglishCenter.DataAccess.Entities
{
    [Table("Ques_Toeic")]
    public class QuesToeic
    {
        [Key]
        public long QuesId { set; get; }

        public long ToeicId { set; get; }

        [ForeignKey("ToeicId")]
        [InverseProperty("QuesToeic")]
        public virtual ToeicExam ToeicExam { set; get; } = null!;

        public string? Audio { set; get; }

        public string? Image_1 { set; get; }

        public string? Image_2 { set; get; }

        public string? Image_3 { set; get; }

        [Required]
        public bool IsGroup { set; get; } = false;

        [Required]
        public int Part { set; get; }

        [Required]
        public int NoNum { set; get; } = 1;

        [InverseProperty("QuesToeic")]
        public virtual ICollection<SubToeic> SubToeicList { set; get; } = new List<SubToeic>();
    }
}
