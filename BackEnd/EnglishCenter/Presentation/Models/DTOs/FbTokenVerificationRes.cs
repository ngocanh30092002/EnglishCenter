namespace EnglishCenter.Presentation.Models.DTOs
{
    public class FbTokenVerificationRes
    {
        public FacebookTokenData Data { get; set; }
    }
    public class FacebookTokenData
    {
        public bool Is_Valid { get; set; }
    }
}
