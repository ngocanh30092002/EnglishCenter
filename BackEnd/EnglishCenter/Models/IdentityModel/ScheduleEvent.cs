using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnglishCenter.Models.IdentityModel
{
    public class ScheduleEvent
    {
        [Key]
        public long ScheduleId { set; get; }
        
        [Required]
        public string Title { set; get; } = null!;

        [Required]    
        public TimeOnly StartTime { set; get; }
        
        [Required]        
        public TimeOnly EndTime { set; get; }
        
        [Required]        
        public DateOnly Date { set; get; }

        public string UserId { set; get; } = null!;

        [ForeignKey("UserId")]
        [InverseProperty("ScheduleEvents")]
        public virtual Student Student { get; set; } = null!;

    }
}
