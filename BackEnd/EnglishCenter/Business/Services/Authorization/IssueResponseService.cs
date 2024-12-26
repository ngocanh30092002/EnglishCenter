using AutoMapper;
using EnglishCenter.Business.IServices;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.UnitOfWork;
using EnglishCenter.Presentation.Global;
using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;
using EnglishCenter.Presentation.Models.ResDTOs;
using Microsoft.AspNetCore.Identity;

namespace EnglishCenter.Business.Services.Authorization
{
    public class IssueResponseService : IIssueResponseService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unit;
        private readonly UserManager<User> _userManager;

        public IssueResponseService(IMapper mapper, IUnitOfWork unit, UserManager<User> userManager)
        {
            _mapper = mapper;
            _unit = unit;
            _userManager = userManager;
        }

        public async Task<Response> CreateAsync(IssueResponseDto model)
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

            var issueModel = _unit.IssueReports.GetById(model.IssueId);
            if (issueModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any issue reports",
                    Success = false
                };
            }

            var issueResponse = new IssueResponse()
            {
                UserId = model.UserId,
                IssueId = model.IssueId,
                Message = model.Message,
                CreatedAt = DateTime.Now
            };

            _unit.IssueResponses.Add(issueResponse);
            await _unit.CompleteAsync();

            await _unit.Notifications.SendNotificationToUser(issueModel.UserId, GlobalVariable.SYSTEM, new NotiDto()
            {
                Title = $"Response Message",
                Description = $"You just received a response from admin",
                Image = "/notifications/images/automatic.svg",
                IsRead = false,
                Time = DateTime.Now,
            });

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "",
                Success = true
            };
        }

        public async Task<Response> DeleteAsync(long id)
        {
            var issueResponse = _unit.IssueResponses.GetById(id);

            if (issueResponse == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any issue response",
                    Success = false
                };
            }

            _unit.IssueResponses.Remove(issueResponse);
            await _unit.CompleteAsync();

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "",
                Success = true
            };
        }

        public Task<Response> GetAllAsync()
        {
            var responses = _unit.IssueResponses.GetAll();

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<IssueResponseResDto>>(responses),
                Success = true
            });
        }

        public Task<Response> GetById(long id)
        {
            var response = _unit.IssueResponses.GetById(id);

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<IssueResponseResDto>(response),
                Success = true
            });
        }

        public async Task<Response> UpdateAsync(long id, IssueResponseDto model)
        {
            var issueResponse = _unit.IssueResponses.GetById(id);

            if (issueResponse == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any issue response",
                    Success = false
                };
            }

            await _unit.BeginTransAsync();

            try
            {
                if (issueResponse.IssueId != model.IssueId)
                {
                    var isChangeSuccess = await _unit.IssueResponses.ChangeIssueAsync(issueResponse, model.IssueId);
                    if (!isChangeSuccess)
                    {
                        return new Response()
                        {
                            StatusCode = System.Net.HttpStatusCode.BadRequest,
                            Message = "Can't change issue report",
                            Success = false
                        };
                    }
                }

                if (issueResponse.Message != model.Message)
                {
                    var isChangeSuccess = await _unit.IssueResponses.ChangeMessageAsync(issueResponse, model.Message);
                    if (!isChangeSuccess)
                    {
                        return new Response()
                        {
                            StatusCode = System.Net.HttpStatusCode.BadRequest,
                            Message = "Can't change message",
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
    }
}
