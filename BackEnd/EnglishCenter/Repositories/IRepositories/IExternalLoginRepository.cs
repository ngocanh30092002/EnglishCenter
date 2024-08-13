using EnglishCenter.Models;

namespace EnglishCenter.Repositories.IRepositories
{
    public interface IExternalLoginRepository
    {
        public Task<Response> SignInGoogleAsync(Dictionary<string, string> requestParams, string googleTokenUrl);

    }
}
