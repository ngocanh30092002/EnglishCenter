using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnglishCenter.DataAccess.Entities;

public class Class
{
    [Key]
    [StringLength(10)]
    public string ClassId { get; set; } = null!;

    [StringLength(10)]
    public string CourseId { get; set; } = null!;

    [StringLength(100)]
    public string TeacherId { get; set; } = null!;

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public int RegisteringNum { set; get; } = 0;

    public int RegisteredNum { get; set; } = 0;

    public int MaxNum { get; set; } = 0;

    public int Status { set; get; } = 0;

    [StringLength(200)]
    public string? Description { get; set; }

    [StringLength(200)]
    public string? Image { set; get; }

    [ForeignKey("CourseId")]
    [InverseProperty("Classes")]
    public virtual Course Course { get; set; } = null!;

    [ForeignKey("TeacherId")]
    [InverseProperty("Classes")]
    public virtual Teacher Teacher { set; get; } = null!;

    [InverseProperty("Class")]
    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

    [InverseProperty("Class")]
    public virtual ICollection<ClassSchedule> ClassSchedules { set; get; } = new List<ClassSchedule>();

    [InverseProperty("Class")]
    public virtual ICollection<Lesson> Lessons { set; get; } = new List<Lesson>();

    [InverseProperty("Class")]
    public virtual ICollection<ClassMaterial> ClassMaterials { set; get; } = new List<ClassMaterial>();
}
