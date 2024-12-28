using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.Business.IServices
{
    public interface IExternalLoginService
    {
        public Task<Response> SignInGoogleAsync(Dictionary<string, string> requestParams, string googleTokenUrl);
        public Task<bool> IsFacebookTokenValidAsync(string userAccessToken);
        public Task<FbUserInfo> GetFbUserInfoAsync(string userAccessToken);
        public Task<Response> SignInFacebookAsync(FbUserInfo userInfo);
    }
}
