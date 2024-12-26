using System.ComponentModel.DataAnnotations;

namespace EnglishCenter.DataAccess.Entities
{
    public class Period
    {
        [Key]
        public int PeriodId { set; get; }

        public string StartTime { set; get; } = null!;

        public string EndTime { set; get; } = null!;
    }
}
