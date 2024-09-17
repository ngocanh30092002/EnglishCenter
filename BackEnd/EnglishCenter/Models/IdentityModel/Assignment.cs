using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EnglishCenter.Models.IdentityModel;

namespace EnglishCenter.Models;

[Table("Assignment")]
public partial class Assignment
{
    [Key]
    public long AssignmentId { get; set; }

    [Required]
    public int NoNum { set; get; }

    [Column(TypeName = "nvarchar(200)")]
    public string? Title { set; get; }

    public TimeOnly? Time { set; get; }

    public long CourseContentId { set; get; }

    [ForeignKey("CourseContentId")]
    [InverseProperty("Assignments")]
    public virtual CourseContent CourseContent { set; get; }

    [InverseProperty("Assignment")]
    public virtual ICollection<AssignQue> AssignQues { set; get; } = new List<AssignQue>();

    [InverseProperty("Assignment")]
    public virtual ICollection<Homework> HomeworkList { get; set; } = new List<Homework>();

}
