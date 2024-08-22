using EnglishCenter.Global.Enum;

namespace EnglishCenter.Models
{
    public class GoogleUserInfo
    {
        public DateTime DayOfBirth { set; get; }    
        public string PhoneNumber { set; get; }
        public string Address { set;get; }  
        public Gender Gender { set; get; }
    }
}
