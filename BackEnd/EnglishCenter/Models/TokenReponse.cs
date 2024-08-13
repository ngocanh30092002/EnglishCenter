using System.Text.Json.Serialization;

namespace EnglishCenter.Models
{
    public class TokenReponse
    {
        [JsonPropertyName("access_token")]
        public string Access_Token { set; get; }

        [JsonPropertyName("expires_in")]
        public int Expires_In { set; get; }

        [JsonPropertyName("refresh_token")]
        public string Refresh_Token { set; get; }

        [JsonPropertyName("scope")]
        public string Scope { set; get; }

        [JsonPropertyName("token_type")]
        public string Token_Type { set; get; }

        [JsonPropertyName("id_token")]
        public string Token_Id { set; get; }
    }
}
