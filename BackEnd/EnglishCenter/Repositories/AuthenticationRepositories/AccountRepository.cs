// Ignore Spelling: Repo

using System.Net;
using System.Security.Claims;
using System.Web;
using Azure.Core;
using EnglishCenter.Controllers.AuthenticationPage;
using EnglishCenter.Database;
using EnglishCenter.Global;
using EnglishCenter.Global.Enum;
using EnglishCenter.Helpers;
using EnglishCenter.Models;
using EnglishCenter.Models.DTO;
using EnglishCenter.Repositories.IRepositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.CodeAnalysis;
using Microsoft.DotNet.Scaffolding.Shared;

namespace EnglishCenter.Repositories.AuthenticationRepositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly EnglishCenterContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IClaimRepository _claimRepo;
        private readonly IConfiguration _configuration;
        private readonly IJsonWebTokenRepository _jwtRepo;
        private readonly MailHelper _mailHelper;
        private readonly LinkGenerator _linkGenerator;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountRepository(
            EnglishCenterContext context,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            RoleManager<IdentityRole> roleManager,
            IClaimRepository claimRepo,
            IJsonWebTokenRepository jwtRepo,
            IConfiguration configuration,
            MailHelper mailHelper,
            LinkGenerator linkGenerator,
            IHttpContextAccessor httpContextAccessor
        ) 
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _claimRepo = claimRepo;
            _configuration = configuration;
            _jwtRepo = jwtRepo;
            _mailHelper = mailHelper;
            _linkGenerator = linkGenerator;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Response> RenewPasswordAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if(user == null)
            {
                return new Response
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "User isn't exits"
                };
            }

            var passwordToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var newPassword = GlobalMethods.GeneratePassword(10);

            var result = await _userManager.ResetPasswordAsync(user, passwordToken, newPassword);

            if (!result.Succeeded)
            {
                return new Response
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = result.Errors.Select(e => e.Description).ToList(),
                };
            }


            // Send Verify code to personal email

            if (_httpContextAccessor.HttpContext != null)
            {
                string htmlCode = $@"Your new password is {newPassword} Please change your password as soon as possible..";
                string subjectTitle = "Renew Password";

                _ = _mailHelper.SendHtmlMailAsync(new MailContent()
                {
                    Body = htmlCode,
                    From = _configuration["MailSettings:Mail"] ?? "",
                    To = email,
                    Subject = subjectTitle
                });
            }

            return new Response
            {
                StatusCode = HttpStatusCode.OK,
                Success = true,
                Message = "Success",
            };
        }

        public async Task<Response> LoginAsync(LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            var isPasswordValid = await _userManager.CheckPasswordAsync(user, model.Password);
            Response response;
            if (user == null)
            {
                return new Response()
                {
                    Message = "User isn't exist",
                    StatusCode = HttpStatusCode.BadRequest
                };
            }

            var lockedResponse = await LockedOutUserWhenLoginAsync(user);
            if (!lockedResponse.Success)
            {
                return lockedResponse;
            }

            if (!isPasswordValid)
            {
                await _userManager.AccessFailedAsync(user);

                return new Response()
                {
                    Message = "Password isn't correct",
                    StatusCode = HttpStatusCode.BadRequest
                };
            }

            var accessToken = await _jwtRepo.GenerateUserTokenAsync(user, DateTime.UtcNow.AddMinutes(GlobalVariable.TOKEN_EXPIRED));
            var refreshToken = await _jwtRepo.GetRefreshTokenFromUser(user);

            if (string.IsNullOrEmpty(refreshToken))
            {
                refreshToken = _jwtRepo.GenerateRefreshToken();
                await _userManager.SetAuthenticationTokenAsync(user, Provider.System.ToString(), GlobalVariable.REFRESH_TOKEN, refreshToken);
            }

            return new Response()
            {
                Success = true,
                Message = "Success",
                Token = accessToken,
                RefreshToken = refreshToken,
                StatusCode = HttpStatusCode.OK,
            };
        }

        public async Task<Response> RegisterAsync(RegisterModel model, Provider provider = Provider.System)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            Response response;
            if (user != null)
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
                PhoneNumber = model?.PhoneNumber
            };

            var result = await _userManager.CreateAsync(newUser, model.Password);

            if (!result.Succeeded)
            {
                return new Response()
                {
                    Message = result.Errors.Select(e => e.Description).ToList(),
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }

            // Add role to user
            if (!await _roleManager.RoleExistsAsync(AppRole.STUDENT))
            {
                await _roleManager.CreateAsync(new IdentityRole(AppRole.STUDENT));
            }

            // Add claims to user
            await _userManager.AddToRoleAsync(newUser, AppRole.STUDENT);
            await _claimRepo.AddClaimToUserAsync(newUser, new ClaimDto(ClaimTypes.Email, newUser.Email));
            await _claimRepo.AddClaimToUserAsync(newUser, new ClaimDto(ClaimTypes.Gender, model.Gender.ToString()));
           
            if(model.DateOfBirth != null)
            {
                await _claimRepo.AddClaimToUserAsync(newUser, new ClaimDto(ClaimTypes.DateOfBirth, model.DateOfBirth.ToString()));
            }

            if (model.PhoneNumber != null)
            {
                await _claimRepo.AddClaimToUserAsync(newUser, new ClaimDto(ClaimTypes.MobilePhone, model.PhoneNumber.ToString()));

                // Verify phone number
                var phoneCode = await _userManager.GenerateChangePhoneNumberTokenAsync(newUser, model.PhoneNumber);
                var isConfirmPhone = await _userManager.VerifyChangePhoneNumberTokenAsync(newUser,phoneCode, model.PhoneNumber);
            }


            // Send Email to User
            var sendEmailResult = SendEmailToUserAsync(newUser, provider, model);

            var createdResponse = await CreateStudentWithUser(newUser, model);

            if (!createdResponse.Success)
            {
                return createdResponse;
            };

            return new Response()
            {
                Success = true,
                Message = "Registered successfully",
                StatusCode = HttpStatusCode.OK
            };
        }

        public async Task<Response> CreateStudentWithUser(User newUser, RegisterModel model)
        {
            var isExistUser = _context.Students.FirstOrDefault(u => u.UserId == newUser.Id);
            if (isExistUser != null)
            {
                return new Response
                {
                    Success = false,
                    Message = "This student already exists in the database",
                    StatusCode = HttpStatusCode.BadRequest
                };
            }

            var studentInfo = new Student()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Gender = (int) model.Gender,
                Address = model?.Address,
                DateOfBirth = model?.DateOfBirth,
                PhoneNumber = model?.PhoneNumber,
                UserId = newUser.Id,
                UserName = model?.LastName
            };

            _context.Students.Add(studentInfo);

            await _context.SaveChangesAsync();
            return new Response() { Success = true };
        }

        public async Task<Response> LockedOutUserWhenLoginAsync(User user)
        {
            bool isLockedOut = await _userManager.IsLockedOutAsync(user);
            if (isLockedOut)
            {
                if (user.LockoutEnd.HasValue)
                {
                    if (user.LockoutEnd.Value > DateTime.UtcNow)
                    {
                        return new Response()
                        {
                            Message = "The account is still locked at this time. Please try again later.",
                            StatusCode = HttpStatusCode.BadRequest,
                            Success = false
                        };
                    }
                    else
                    {
                        var result = await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow);
                        if (result.Succeeded)
                        {
                            await _userManager.ResetAccessFailedCountAsync(user);
                            return new Response()
                            {
                                Message = "",
                                StatusCode = HttpStatusCode.OK,
                                Success = true
                            };
                        }
                    }
                }
            }

            return new Response() { Success = true};
        }

        private async Task SendEmailToUserAsync(User newUser, Provider provider, RegisterModel model)
        {
            // Send Verify code to personal email
            string? htmlLink = "";
            string htmlCode = "";
            string subjectTitle = "";
            Task<bool> sendResult;

            if (_httpContextAccessor.HttpContext != null && provider == Provider.System)
            {
                var emailCode = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);

                htmlLink = _linkGenerator.GetUriByAction(
                    _httpContextAccessor.HttpContext,
                    action: nameof(ConfirmEmailController.ExecuteConfirmEmailAsync).Replace("Async", ""),
                    controller: nameof(ConfirmEmailController).Replace("Controller", ""),
                    values: new { userId = newUser.Id, code = emailCode, returnUrl = HttpUtility.UrlEncode(GlobalVariable.CLIENT_URL) }
                );

                htmlCode = $@"
                Confirm your email address when logging into our information system. 
                <a href ='{htmlLink}'>Click this button</a>";

                subjectTitle = "Email Confirmation For English Center";
            }
            else if (_httpContextAccessor.HttpContext != null && provider == Provider.Google)
            {
                htmlCode = $"Your password is {model.Password}";

                subjectTitle = "Password Of English Center";
            }


            _ = _mailHelper.SendHtmlMailAsync(new MailContent()
            {
                Body = htmlCode,
                From = _configuration["MailSettings:Mail"] ?? "",
                To = model.Email,
                Subject = subjectTitle
            });
        }
    }
}
