using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnglishCenter.DataAccess.Entities
{
    public class NotiStudent
    {
        [Key]
        public long NotiStuId { set; get; }
        public long NotiId { get; set; }

        [ForeignKey("NotiId")]
        [InverseProperty("NotiStudents")]
        public virtual Notification? Notification { get; set; }

        public string UserId { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("NotiStudents")]
        public virtual Student? Student { get; set; }

        public bool IsRead { get; set; }
    }
}
