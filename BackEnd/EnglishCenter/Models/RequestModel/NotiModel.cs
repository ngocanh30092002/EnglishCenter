using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnglishCenter.Models.RequestModel
{
    public class NotiModel
    {
        public string Title { set; get; }
        public string? Description { get; set; }
        public string? Image { set; get; }
        public string? LinkUrl { set; get; }
    }
}
