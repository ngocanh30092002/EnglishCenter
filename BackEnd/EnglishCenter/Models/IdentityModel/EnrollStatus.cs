using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.Models;

[Table("EnrollStatus")]
public partial class EnrollStatus
{
    [Key]
    public int StatusId { get; set; }

    [StringLength(50)]
    public string? Name { get; set; }

    [InverseProperty("Status")]
    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}
