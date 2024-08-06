using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace EnglishCenter.Models
{
    public class User : IdentityUser
    {
        public virtual Student? Student { get; set; }
    }
}
