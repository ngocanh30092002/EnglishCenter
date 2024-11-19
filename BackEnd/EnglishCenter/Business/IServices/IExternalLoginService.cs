using EnglishCenter.Presentation.Models;

namespace EnglishCenter.Business.IServices
{
    public interface IExternalLoginService
    {
        public Task<Response> SignInGoogleAsync(Dictionary<string, string> requestParams, string googleTokenUrl);
    }
}
