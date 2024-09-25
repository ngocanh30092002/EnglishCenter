using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnglishCenter.DataAccess.Entities;

public partial class Notification
{
    [Key]
    public long NotiId { get; set; }

    [Required]
    [StringLength(100)]
    public string Title { get; set; }

    [Required]
    [StringLength(200)]
    public string Description { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? Time { get; set; }

    [StringLength(200)]
    public string? Image { set; get; }

    [StringLength(300)]
    public string? LinkUrl { set; get; }

    [InverseProperty("Notification")]
    public virtual ICollection<NotiStudent> NotiStudents { set; get; } = new List<NotiStudent>();
}
