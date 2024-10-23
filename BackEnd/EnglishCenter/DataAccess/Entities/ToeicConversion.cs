using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnglishCenter.DataAccess.Entities
{
    [Table("Toeic_Conversion")]
    public class ToeicConversion
    {
        [Key]
        public int Id { set; get; }

        public int NumberCorrect { set; get; }

        public int EstimatedScore { set; get; }

        [Column(TypeName = "nvarchar(20)")]
        public string Section { set; get; } = null!;
    }
}
