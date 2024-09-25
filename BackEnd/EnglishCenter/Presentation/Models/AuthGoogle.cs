using System.Text.Json.Serialization;

namespace EnglishCenter.Presentation.Models
{
    public class AuthGoogle
    {
        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("scope")]
        public string Scope { get; set; }

        [JsonPropertyName("authuser")]
        public int AuthUser { get; set; }

        [JsonPropertyName("prompt")]
        public string Prompt { get; set; }
    }
}