using EnglishCenter.DataAccess.Entities;
using EnglishCenter.Presentation.Global.Enum;
using EnglishCenter.Presentation.Models;

namespace EnglishCenter.Business.IServices
{
    public interface IAccountService
    {
        public Task<Response> RenewPasswordAsync(string email);
        public Task<Response> LoginAsync(LoginModel model);
        public Task<Response> RegisterAsync(RegisterModel model, Provider provider = Provider.System);
        public Task<Response> RegisterWithRoleAsync(RegisterModel model, Provider provider = Provider.System);
        public Task<Response> LockedOutUserAsync(User user);
    }
}
