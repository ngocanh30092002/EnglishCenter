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
        private readonly UserManager<UserAccount> _userManager;
        private readonly SignInManager<UserAccount> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IClaimRepository _claimRepo;
        private readonly IConfiguration _configuration;

        public AccountRepository(
            UserManager<UserAccount> userManager,
            SignInManager<UserAccount> signInManager,
            RoleManager<IdentityRole> roleManager,
            IClaimRepository claimRepo,
            IConfiguration configuration
        ) 
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _claimRepo = claimRepo;
            _configuration = configuration;
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

            var tokenKey = await GenerateToken(user);

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

            var newUser = new UserAccount()
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

        private async Task<string> GenerateToken(UserAccount user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var claims = await _claimRepo.GetClaims(user);
            var authenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            claims.Add(new Claim(ClaimTypes.Name, user.UserName));
            claims.Add(new Claim("Id", user.Id));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

            var tokenDes = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(authenKey, SecurityAlgorithms.HmacSha512)
            };

            var securityToken = jwtTokenHandler.CreateToken(tokenDes);

            return jwtTokenHandler.WriteToken(securityToken);
        }
    }
}
