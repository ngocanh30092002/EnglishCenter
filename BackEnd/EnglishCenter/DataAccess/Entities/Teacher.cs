using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnglishCenter.DataAccess.Entities
{
    public class Teacher
    {
        [Key]
        [StringLength(100)]
        public string UserId { get; set; } = null!;

        [StringLength(100)]
        public string? FirstName { get; set; }

        [StringLength(100)]
        public string? LastName { get; set; }

        public int? Gender { get; set; }

        [StringLength(50)]
        public string? PhoneNumber { get; set; }

        [StringLength(200)]
        public string? Address { get; set; }

        [StringLength(200)]
        public string? Image { get; set; }

        [StringLength(200)]
        public string? BackgroundImage { set; get; }

        [StringLength(200)]
        public string? UserName { set; get; }

        [StringLength(300)]
        public string? Description { set; get; }

        public DateOnly? DateOfBirth { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("Teacher")]
        public virtual User User { get; set; } = null!;

        [InverseProperty("Teacher")]
        public virtual ICollection<Class> Classes { set; get; } = new List<Class>();
    }
}
