using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnglishCenter.DataAccess.Entities
{
    [Table("Home_Ques")]
    public class HomeQue
    {
        [Key]
        public long HomeQuesId { set; get; }

        [Required]
        public int Type { set; get; }

        public int NoNum { set; get; } = 1;

        [Column("ImageQues_Id")]
        public long? ImageQuesId { get; set; } = null;

        [ForeignKey("ImageQuesId")]
        [InverseProperty("HomeQues")]
        public virtual QuesLcImage? QuesImage { get; set; }

        [Column("AudioQues_Id")]
        public long? AudioQuesId { set; get; } = null;

        [ForeignKey("AudioQuesId")]
        [InverseProperty("HomeQues")]
        public virtual QuesLcAudio? QuesAudio { get; set; }

        [Column("ConversationQues_Id")]
        public long? ConversationQuesId { set; get; } = null;

        [ForeignKey("ConversationQuesId")]
        [InverseProperty("HomeQues")]
        public virtual QuesLcConversation? QuesConversation { get; set; }

        [Column("SingleQues_Id")]
        public long? SingleQuesId { set; get; } = null;

        [ForeignKey("SingleQuesId")]
        [InverseProperty("HomeQues")]
        public virtual QuesRcSingle? QuesSingle { get; set; }

        [Column("DoubleQues_Id")]
        public long? DoubleQuesId { set; get; } = null;

        [ForeignKey("DoubleQuesId")]
        [InverseProperty("HomeQues")]
        public virtual QuesRcDouble? QuesDouble { get; set; }

        [Column("TripleQues_Id")]
        public long? TripleQuesId { set; get; } = null;

        [ForeignKey("TripleQuesId")]
        [InverseProperty("HomeQues")]
        public virtual QuesRcTriple? QuesTriple { get; set; }

        [Column("SentenceQuesId")]
        public long? SentenceQuesId { set; get; } = null;

        [ForeignKey("SentenceQuesId")]
        [InverseProperty("HomeQues")]
        public virtual QuesRcSentence? QuesSentence { get; set; }

        public long HomeworkId { set; get; }

        [ForeignKey("HomeworkId")]
        [InverseProperty("HomeQues")]
        public virtual Homework Homework { set; get; } = null!;

        [InverseProperty("HomeQue")]
        public virtual ICollection<HwSubRecord> SubRecords { set; get; } = new List<HwSubRecord>();
    }
}
