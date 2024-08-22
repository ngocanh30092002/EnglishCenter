using EnglishCenter.Global.Enum;
using Microsoft.AspNetCore.Mvc;

namespace EnglishCenter.Models
{
    public class TokenRequest
    {
        public string AccessToken { set; get; }
        public string RefreshToken { set; get; }
    }
}
