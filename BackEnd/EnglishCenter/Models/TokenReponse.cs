using Newtonsoft.Json;

namespace EnglishCenter.Models
{
    public class TokenReponse
    {
        [JsonProperty("access_token")]
        public string Access_Token { set; get; }

        [JsonProperty("expires_in")]
        public int Expires_In { set; get; }

        [JsonProperty("refresh_token")]
        public string Refresh_Token { set; get; }

        [JsonProperty("scope")]
        public string Scope { set; get; }

        [JsonProperty("token_type")]
        public string Token_Type { set; get; }

        [JsonProperty("id_token")]
        public string Token_Id { set; get; }
    }
}
