using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnglishCenter.DataAccess.Entities
{
    [Table("UserWords")]
    public class UserWord
    {
        public long UserWordId { set; get; }
        [StringLength(100)]
        public string UserId { set; get; } = null!;

        [ForeignKey("UserId")]
        [InverseProperty("UserWords")]
        public User User { get; set; } = null!;
        public string Word { set; get; } = null!;
        public string Translation { set; get; } = null!;
        public string? Image { set; get; }
        public string Tag { set; get; } = null!;
        public int Type { set; get; }
        public string Phonetic { set; get; } = null!;
        public bool IsFavorite { set; get; } = false;
        public DateTime UpdateDate { set; get; }
    }
}
