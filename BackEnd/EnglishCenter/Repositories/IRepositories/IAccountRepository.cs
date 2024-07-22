using EnglishCenter.Models;
using Microsoft.AspNetCore.Identity;

namespace EnglishCenter.Repositories.IRepositories
{
    public interface IAccountRepository
    {
        public Task<Response> RegisterAsync(RegisterModel model);
        public Task<Response> LoginAsync(LoginModel model);
    }
}
