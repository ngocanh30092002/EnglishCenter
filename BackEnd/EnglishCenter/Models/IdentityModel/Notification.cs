using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.Models;

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

    public bool? IsRead { get; set; }

    [StringLength(200)]
    public string? Image { set; get; }

    [StringLength(300)]
    public string? LinkUrl { set; get;}

    public virtual ICollection<Student> Students { set; get; } = new List<Student>();
}
