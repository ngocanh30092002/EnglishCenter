using EnglishCenter.Global.Enum;
using EnglishCenter.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EnglishCenter.Repositories.IRepositories
{
    public interface IAccountRepository
    {
        public Task<Response> RegisterAsync(RegisterModel model, Provider provider = Provider.System);
        public Task<Response> LoginAsync(LoginModel model);
        public Task<Response> ForgotPasswordAsync(string email);
    }
}
