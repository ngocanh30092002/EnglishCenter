using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.DataAccess.Entities;

[Table("Assign_Ques")]
public class AssignQue
{
    [Key]
    public long AssignQuesId { get; set; }

    [Required]
    public int Type { get; set; }
    
    public int NoNum { set; get; } = 1;

    [Column("ImageQues_Id")]
    public long? ImageQuesId { get; set; } = null;

    [ForeignKey("ImageQuesId")]
    [InverseProperty("AssignQues")]
    public virtual QuesLcImage? QuesImage { get; set; }

    [Column("AudioQues_Id")]
    public long? AudioQuesId { set; get; } = null;

    [ForeignKey("AudioQuesId")]
    [InverseProperty("AssignQues")]
    public virtual QuesLcAudio? QuesAudio { get; set; }

    [Column("ConversationQues_Id")]
    public long? ConversationQuesId { set; get; } = null;

    [ForeignKey("ConversationQuesId")]
    [InverseProperty("AssignQues")]
    public virtual QuesLcConversation? QuesConversation { get; set; }

    [Column("SingleQues_Id")]
    public long? SingleQuesId { set; get; } = null;

    [ForeignKey("SingleQuesId")]
    [InverseProperty("AssignQues")]
    public virtual QuesRcSingle? QuesSingle { get; set; }

    [Column("DoubleQues_Id")]
    public long? DoubleQuesId { set; get; } = null;

    [ForeignKey("DoubleQuesId")]
    [InverseProperty("AssignQues")]
    public virtual QuesRcDouble? QuesDouble { get; set; }

    [Column("TripleQues_Id")]
    public long? TripleQuesId { set; get; } = null;

    [ForeignKey("TripleQuesId")]
    [InverseProperty("AssignQues")]
    public virtual QuesRcTriple? QuesTriple { get; set; }

    [Column("SentenceQuesId")]
    public long? SentenceQuesId { set; get; } = null;

    [ForeignKey("SentenceQuesId")]
    [InverseProperty("AssignQues")]
    public virtual QuesRcSentence? QuesSentence { get; set; }

    [Column("SentenceMediaQuesId")]
    public long? SentenceMediaQuesId { set; get; } = null;

    [ForeignKey("SentenceMediaQuesId")]
    [InverseProperty("AssignQues")]
    public virtual QuesRcSentenceMedia? QuesSentenceMedia { get; set; }

    public long AssignmentId { set; get; }

    [ForeignKey("AssignmentId")]
    [InverseProperty("AssignQues")]
    public virtual Assignment Assignment { set; get; } = null!;

    [InverseProperty("AssignQue")]
    public virtual ICollection<AnswerRecord> AnswerRecords { set; get; } = new List<AnswerRecord>();
}
