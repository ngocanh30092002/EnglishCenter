using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnglishCenter.DataAccess.Entities;

public partial class Class
{
    [Key]
    [StringLength(10)]
    public string ClassId { get; set; } = null!;

    [StringLength(10)]
    public string CourseId { get; set; } = null!;

    [StringLength(100)]
    public string? TeacherId { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public int RegisteringNum { set; get; } = 0;

    public int RegisteredNum { get; set; } = 0;

    public int MaxNum { get; set; } = 0;

    [StringLength(200)]
    public string? Image { set; get; }

    [ForeignKey("CourseId")]
    [InverseProperty("Classes")]
    public virtual Course Course { get; set; } = null!;

    [InverseProperty("Class")]
    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

}
