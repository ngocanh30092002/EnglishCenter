using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnglishCenter.DataAccess.Entities
{
    [Table("ClassRooms")]
    public class ClassRoom
    {
        [Key]
        public long ClassRoomId { set; get; }

        public string ClassRoomName { set; get; } = null!;

        public int Capacity { set; get; }

        public string? Location { set; get; }

        [InverseProperty("ClassRoom")]
        public virtual ICollection<ClassSchedule> ClassSchedules { set; get; } = new List<ClassSchedule>();

        [InverseProperty("ClassRoom")]
        public virtual ICollection<Lesson> Lessons { set; get; } = new List<Lesson>();
    }
}
