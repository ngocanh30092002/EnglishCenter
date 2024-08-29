using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.Models;

public class User : IdentityUser
{
    [InverseProperty("User")]
    public virtual Student? Student { get; set; }

}
