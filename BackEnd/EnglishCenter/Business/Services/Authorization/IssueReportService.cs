using AutoMapper;
using EnglishCenter.Business.IServices;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.UnitOfWork;
using EnglishCenter.Presentation.Global;
using EnglishCenter.Presentation.Global.Enum;
using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;
using EnglishCenter.Presentation.Models.ResDTOs;
using Microsoft.AspNetCore.Identity;

namespace EnglishCenter.Business.Services.Authorization
{
    public class IssueReportService : IIssueReportService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unit;
        private readonly UserManager<User> _userManager;
        private readonly IUserService _userService;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IIssueResponseService _issueResService;

        public IssueReportService(
            IMapper mapper,
            IUnitOfWork unit,
            UserManager<User> userManager,
            IUserService userService,
            RoleManager<IdentityRole> roleManager,
            IIssueResponseService issueResService
            )
        {
            _mapper = mapper;
            _unit = unit;
            _userManager = userManager;
            _userService = userService;
            _roleManager = roleManager;
            _issueResService = issueResService;
        }

        public async Task<Response> CreateAsync(IssueReportDto model)
        {
            if (string.IsNullOrEmpty(model.UserId))
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "User Id is required",
                    Success = false
                };
            }

            var userModel = await _userManager.FindByIdAsync(model.UserId);
            if (userModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any users",
                    Success = false
                };
            }

            if (!Enum.IsDefined(typeof(IssueTypeEnum), model.Type))
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Type is invalid",
                    Success = false
                };
            }

            var issueModel = new IssueReport()
            {
                Title = model.Title,
                Description = model.Description,
                Type = model.Type,
                CreatedAt = DateTime.Now,
                Status = (int)IssueStatusEnum.Open,
                UserId = model.UserId,
            };

            _unit.IssueReports.Add(issueModel);
            await _unit.CompleteAsync();


            var adminUsers = await _userManager.GetUsersInRoleAsync(AppRole.ADMIN);

            foreach (var admin in adminUsers)
            {
                await _unit.Notifications.SendNotificationToUser(admin.Id, GlobalVariable.SYSTEM, new NotiDto()
                {
                    Title = $"{((IssueTypeEnum)issueModel.Type).ToString()} Report Message",
                    Description = $"You just received an issue report from a user.",
                    Image = "/notifications/images/automatic.svg",
                    IsRead = false,
                    Time = DateTime.Now,
                });
            }

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "",
                Success = true
            };
        }

        public async Task<Response> DeleteAsync(long id)
        {
            var issueModel = _unit.IssueReports.GetById(id);
            if (issueModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any issue reports",
                    Success = false
                };
            }

            var responseIds = _unit.IssueResponses
                                   .Find(r => r.IssueId == issueModel.IssueId)
                                   .Select(s => s.IssueResId)
                                   .ToList();

            await _unit.BeginTransAsync();

            try
            {
                foreach (var resId in responseIds)
                {
                    var deleteRes = await _issueResService.DeleteAsync(resId);
                    if (!deleteRes.Success) return deleteRes;
                }


                _unit.IssueReports.Remove(issueModel);
                await _unit.CompleteAsync();
                await _unit.CommitTransAsync();

                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Message = "",
                    Success = true
                };
            }
            catch (Exception ex)
            {
                await _unit.RollBackTransAsync();
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = ex.Message,
                    Success = false
                };
            }
        }

        public async Task<Response> GetAllAsync()
        {
            var issueReports = _unit.IssueReports.GetAll();

            var resDtos = new List<IssueReportResDto>();

            foreach (var issue in issueReports)
            {
                var resDto = _mapper.Map<IssueReportResDto>(issue);
                var userInfoRes = await _userService.GetUserFullInfoAsync(issue.UserId);
                var userInfo = userInfoRes.Message as UserInfoResDto;

                resDto.UserName = userInfo?.UserName;
                resDto.Email = userInfo?.Email;
                resDto.Image = userInfo?.Image;

                resDtos.Add(resDto);
            }

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = resDtos,
                Success = true
            };
        }

        public async Task<Response> GetById(long id)
        {
            var issueReport = _unit.IssueReports.Include(r => r.IssueResponses).FirstOrDefault(r => r.IssueId == id);

            var resDto = _mapper.Map<IssueReportResDto>(issueReport);
            var userInfoRes = await _userService.GetUserFullInfoAsync(issueReport.UserId);
            var userInfo = userInfoRes.Message as UserInfoResDto;

            resDto.UserName = userInfo?.UserName;
            resDto.Email = userInfo?.Email;
            resDto.Image = userInfo?.Image;

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = resDto,
                Success = true
            };
        }

        public async Task<Response> GetByUserAsync(string userId)
        {
            var issueReports = _unit.IssueReports
                                    .Include(r => r.IssueResponses)
                                    .Where(r => r.UserId == userId)
                                    .ToList();

            var resDtos = new List<IssueReportResDto>();

            foreach (var issue in issueReports)
            {
                var resDto = _mapper.Map<IssueReportResDto>(issue);
                var userInfoRes = await _userService.GetUserFullInfoAsync(issue.UserId);
                var userInfo = userInfoRes.Message as UserInfoResDto;

                resDto.UserName = userInfo?.UserName;
                resDto.Email = userInfo?.Email;
                resDto.Image = userInfo?.Image;

                var responses = new List<IssueResponseResDto>();
                foreach (var response in issue.IssueResponses)
                {
                    var responseResDto = _mapper.Map<IssueResponseResDto>(response);
                    var userResBgRes = await _userService.GetUserBackgroundInfoAsync(response.UserId);
                    var userResBgInfo = userResBgRes.Message as UserBackgroundDto;

                    responseResDto.Image = userResBgInfo?.Image;

                    responses.Add(responseResDto);
                }

                resDto.Responses = responses;
                resDtos.Add(resDto);
            }

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = resDtos,
                Success = true
            };
        }

        public Task<Response> GetStatusAsync()
        {
            var typeQues = Enum.GetValues(typeof(IssueStatusEnum))
                           .Cast<IssueStatusEnum>()
                           .Select(type => new KeyValuePair<string, int>(type.ToString(), (int)type))
                           .ToList();

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = typeQues,
                Success = true
            });
        }

        public Task<Response> GetTypeAsync()
        {
            var typeQues = Enum.GetValues(typeof(IssueTypeEnum))
                           .Cast<IssueTypeEnum>()
                           .Select(type => new KeyValuePair<string, int>(type.ToString(), (int)type))
                           .ToList();

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = typeQues,
                Success = true
            });
        }

        public async Task<Response> GetAllByAdminAsync()
        {
            var issueReports = _unit.IssueReports.Include(i => i.IssueResponses).OrderByDescending(i => i.CreatedAt).ToList();

            var resDtos = new List<IssueReportResDto>();

            foreach (var issue in issueReports)
            {
                var resDto = _mapper.Map<IssueReportResDto>(issue);
                var userInfoRes = await _userService.GetUserFullInfoAsync(issue.UserId);
                var userBgRes = await _userService.GetUserBackgroundInfoAsync(issue.UserId);

                var userInfo = userInfoRes.Message as UserInfoResDto;
                var userBgInfo = userBgRes.Message as UserBackgroundDto;

                resDto.UserName = userInfo?.UserName;
                resDto.Email = userInfo?.Email;
                resDto.Image = userInfo?.Image;
                resDto.Roles = userBgInfo?.Roles;

                var responses = new List<IssueResponseResDto>();
                foreach (var response in issue.IssueResponses)
                {
                    var responseResDto = _mapper.Map<IssueResponseResDto>(response);
                    var userResBgRes = await _userService.GetUserBackgroundInfoAsync(response.UserId);
                    var userResBgInfo = userResBgRes.Message as UserBackgroundDto;

                    responseResDto.Image = userResBgInfo?.Image;

                    responses.Add(responseResDto);
                }

                resDto.Responses = responses;
                resDtos.Add(resDto);
            }

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = resDtos,
                Success = true
            };
        }

        public async Task<Response> UpdateAsync(long id, IssueReportDto model)
        {
            var issueModel = _unit.IssueReports.GetById(id);
            if (issueModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any issue reports",
                    Success = false
                };
            }

            await _unit.BeginTransAsync();

            try
            {
                if (issueModel.Title != model.Title)
                {
                    var isChangeSuccess = await _unit.IssueReports.ChangeTitleAsync(issueModel, model.Title);
                    if (!isChangeSuccess)
                    {
                        return new Response()
                        {
                            StatusCode = System.Net.HttpStatusCode.BadRequest,
                            Message = "Can't change title",
                            Success = false
                        };
                    }
                }

                if (!string.IsNullOrEmpty(model.Description) && issueModel.Description != model.Description)
                {
                    var isChangeSuccess = await _unit.IssueReports.ChangeDesAsync(issueModel, model.Description);
                    if (!isChangeSuccess)
                    {
                        return new Response()
                        {
                            StatusCode = System.Net.HttpStatusCode.BadRequest,
                            Message = "Can't change description",
                            Success = false
                        };
                    }
                }

                if (issueModel.Type != model.Type)
                {
                    var isChangeSuccess = await _unit.IssueReports.ChangeTypeAsync(issueModel, model.Type);
                    if (!isChangeSuccess)
                    {
                        return new Response()
                        {
                            StatusCode = System.Net.HttpStatusCode.BadRequest,
                            Message = "Can't change type",
                            Success = false
                        };
                    }
                }

                await _unit.CompleteAsync();
                await _unit.CommitTransAsync();

                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Message = "",
                    Success = true
                };
            }
            catch (Exception ex)
            {
                await _unit.RollBackTransAsync();

                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = ex.Message,
                    Success = false
                };
            }
        }

        public async Task<Response> ChangeStatusAsync(long id, int status)
        {
            var issueModel = _unit.IssueReports.GetById(id);
            if (issueModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any issue reports",
                    Success = false
                };
            }

            if (!Enum.IsDefined(typeof(IssueStatusEnum), status))
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Status is invalid",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.IssueReports.ChangeStatusAsync(issueModel, status);
            if (isChangeSuccess == false)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't change status ",
                    Success = false
                };
            }

            await _unit.CompleteAsync();

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "",
                Success = true
            };
        }
    }
}
