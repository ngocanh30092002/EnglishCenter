using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnglishCenter.DataAccess.Entities
{
    [Table("ClassSchedules")]
    public class ClassSchedule
    {
        [Key]
        public long ScheduleId { set; get; }

        [StringLength(10)]
        public string ClassId { set; get; } = null!;

        [ForeignKey("ClassId")]
        [InverseProperty("ClassSchedules")]
        public virtual Class Class { set; get; } = null!;

        [Range(0, 6)]
        public int DayOfWeek { set; get; }

        [Range(1, 12)]
        public int StartPeriod { set; get; }

        [Range(1, 12)]
        public int EndPeriod { set; get; }

        public long ClassRoomId { set; get; }

        [ForeignKey("ClassRoomId")]
        [InverseProperty("ClassSchedules")]
        public virtual ClassRoom ClassRoom { set; get; } = null!;

        public bool IsActive { set; get; }
    }
}
