using System.Security.Claims;

namespace EnglishCenter.Presentation.Models
{
    public class DecodeJwtToken
    {
        public string KeyId { set; get; }
        public string Issuer { set; get; }
        public List<string> Audience { set; get; }
        public List<Claim> Claims { set; get; }
        public DateTime ExpireDate { set; get; }
        public DateTime ValidFrom { set; get; }
        public string SigningAlgorithm { set; get; }
        public string RawData { set; get; }
        public string Subject { set; get; }
        public string Header { set; get; }
        public string Payload { set; get; }
    }
}
