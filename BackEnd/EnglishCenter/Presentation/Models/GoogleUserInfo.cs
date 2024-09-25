using EnglishCenter.Presentation.Global.Enum;

namespace EnglishCenter.Presentation.Models
{
    public class GoogleUserInfo
    {
        public DateTime DayOfBirth { set; get; }
        public string PhoneNumber { set; get; }
        public string Address { set; get; }
        public Gender Gender { set; get; }
    }
}
