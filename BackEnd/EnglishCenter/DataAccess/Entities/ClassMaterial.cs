using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnglishCenter.DataAccess.Entities
{
    [Table("ClassMaterials")]
    public class ClassMaterial
    {
        [Key]
        public long ClassMaterialId { set; get; }
        public string Title { set; get; } = null!;
        public string FilePath { set; get; } = null!;
        public DateTime UploadAt { set; get; }
        public string UploadBy { set; get; } = null!;

        [StringLength(10)]
        public string? ClassId { set; get; }

        [ForeignKey("ClassId")]
        [InverseProperty("ClassMaterials")]
        public virtual Class? Class { set; get; }

        public long? LessonId { set; get; }
        [ForeignKey("LessonId")]
        [InverseProperty("ClassMaterials")]
        public virtual Lesson? Lesson { set; get; }
    }
}
