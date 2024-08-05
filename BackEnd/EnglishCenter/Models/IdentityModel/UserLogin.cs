using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.Models;

[PrimaryKey("LoginProvider", "ProviderKey")]
[Index("UserId", Name = "IX_UserLogins_UserId")]
public partial class UserLogin
{
    [Key]
    [StringLength(100)]
    public string LoginProvider { get; set; } = null!;

    [Key]
    [StringLength(100)]
    public string ProviderKey { get; set; } = null!;

    public string? ProviderDisplayName { get; set; }

    [StringLength(100)]
    public string UserId { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("UserLogins")]
    public virtual User User { get; set; } = null!;
}
