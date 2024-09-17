using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.Models;

[Table("ScoreHistory")]
public partial class ScoreHistory
{
    [Key]
    public long ScoreHisId { get; set; }

    public int? EntrancePoint { get; set; }

    public int? MidtermPoint { get; set; }

    public int? FinalPoint { get; set; }

    [InverseProperty("ScoreHis")]
    public virtual StuInClass? StuInClass { get; set; }
}
