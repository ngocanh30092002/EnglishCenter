using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace EnglishCenter.DataAccess.Entities;

public class User : IdentityUser
{
    [InverseProperty("User")]
    public virtual Student? Student { get; set; }

    [InverseProperty("User")]
    public virtual Teacher? Teacher { set; get; }

}
