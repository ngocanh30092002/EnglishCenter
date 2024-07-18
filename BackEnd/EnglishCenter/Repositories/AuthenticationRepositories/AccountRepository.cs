using EnglishCenter.Models;
using EnglishCenter.Repositories.IRepositories;
using Microsoft.AspNetCore.Identity;

namespace EnglishCenter.Repositories.AuthenticationRepositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<UserAccount> _userManager;
        private readonly SignInManager<UserAccount> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountRepository(
            UserManager<UserAccount> userManager,
            SignInManager<UserAccount> signInManager,
            RoleManager<IdentityRole> roleManager
        ) 
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        public async Task<Response> LoginAsync(LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null) return new Response();

            return new Response();
        }

        public Task<IdentityResult> RegisterAsync(RegisterModel model)
        {
            throw new NotImplementedException();
        }
    }
}
