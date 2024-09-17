using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EnglishCenter.Models.IdentityModel;

namespace EnglishCenter.Models;

public partial class Course
{
    [Key]
    [StringLength(10)]
    public string CourseId { get; set; } = null!;

    [StringLength(50)]
    public string? Name { get; set; }

    [StringLength(200)]
    public string? Description { get; set; }

    public int? NumLesson { get; set; }

    public int? EntryPoint { get; set; }

    public int? StandardPoint { get; set; }

    public int? Priority { get; set; }

    [StringLength(300)]
    public string? Image { set; get; }

    [InverseProperty("Course")]
    public virtual ICollection<Class> Classes { get; set; } = new List<Class>();

    [InverseProperty("Course")]
    public virtual ICollection<CourseContent> CourseContents { set; get; } = new List<CourseContent>();
}
