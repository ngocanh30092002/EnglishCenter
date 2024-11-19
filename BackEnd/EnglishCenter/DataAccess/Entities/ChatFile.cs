using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnglishCenter.DataAccess.Entities
{
    public class ChatFile
    {
        [Key]
        public long FileId { set; get; }
        public string FileName { set; get; }
        public string FilePath { set; get; }
        public string FileType { set; get; }

        [InverseProperty("ChatFile")]
        public ChatMessage ChatMessage { set; get; } = null!;
    }
}
