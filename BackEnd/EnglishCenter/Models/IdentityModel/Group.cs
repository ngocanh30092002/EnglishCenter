using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnglishCenter.Models
{
    public class Group
    {
        public int GroupId { set; get; }

        [StringLength(100)]
        public string Name { set; get; }

        public virtual ICollection<Student> Students { set; get; } = new List<Student>();
    }
}
