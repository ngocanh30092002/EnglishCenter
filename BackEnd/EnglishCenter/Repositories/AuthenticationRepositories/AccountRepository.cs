// Ignore Spelling: Repo

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EnglishCenter.Models;
using EnglishCenter.Repositories.IRepositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using Microsoft.AspNetCore.Http.HttpResults;
using EnglishCenter.Global;

namespace EnglishCenter.Repositories.AuthenticationRepositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IClaimRepository _claimRepo;
        private readonly IConfiguration _configuration;
        private readonly IJsonWebTokenRepository _jwtRepo;

        public AccountRepository(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            RoleManager<IdentityRole> roleManager,
            IClaimRepository claimRepo,
            IJsonWebTokenRepository jwtRepo,
            IConfiguration configuration
        ) 
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _claimRepo = claimRepo;
            _configuration = configuration;
            _jwtRepo = jwtRepo;
        }

        public async Task<Response> LoginAsync(LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            var isPasswordValid = await _userManager.CheckPasswordAsync(user, model.Password);

            if (user == null)
            {
                return new Response() {
                    Message = "User isn't exist",
                    StatusCode = HttpStatusCode.BadRequest
                };
            }
            if (!isPasswordValid)
            {
                return new Response()
                {
                    Message = "Password isn't correct",
                    StatusCode = HttpStatusCode.BadRequest
                };
            }

            var tokenKey = await _jwtRepo.GenerateUserTokenAsync(user, DateTime.UtcNow.AddMinutes(10));

            return new Response() { 
                Success = true,
                Message = "Success",
                Token = tokenKey,
                StatusCode = HttpStatusCode.OK,
            };
        }

        public async Task<Response> RegisterAsync(RegisterModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            if(user != null)
            {
                return new Response()
                {
                    Message = "User already exists",
                    StatusCode = HttpStatusCode.BadRequest,
                };
            }

            var newUser = new User()
            {
                UserName = model.UserName,
                Email = model.Email,
            };

            var result = await _userManager.CreateAsync(newUser, model.Password);
            if(!result.Succeeded)
            {
                return new Response()
                {
                    Message = string.Join("<br>", result.Errors.Select(e => e.Description).ToList()),
                    StatusCode = HttpStatusCode.BadRequest
                };
            }

            if (!await _roleManager.RoleExistsAsync(AppRole.STUDENT))
            {
                await _roleManager.CreateAsync(new IdentityRole(AppRole.STUDENT));
            }

            await _userManager.AddToRoleAsync(newUser, AppRole.STUDENT);
            await _claimRepo.AddClaimToUser(newUser, ClaimTypes.Email, newUser.Email);
            await _claimRepo.AddClaimToUser(newUser, ClaimTypes.Gender, model.Gender.ToString());
            await _claimRepo.AddClaimToUser(newUser, ClaimTypes.DateOfBirth, model.DateOfBirth.ToString()); 

            return new Response()
            {
                Success = true,
                Message = "Registered successfully",
                StatusCode = HttpStatusCode.OK
            };
        }
    }
}
