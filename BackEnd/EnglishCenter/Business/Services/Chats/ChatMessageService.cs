using AutoMapper;
using EnglishCenter.Business.IServices;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.UnitOfWork;
using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;
using EnglishCenter.Presentation.Models.ResDTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.Business.Services.Chats
{
    public class ChatMessageService : IChatMessageService
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<User> _userManager;

        public ChatMessageService(IUnitOfWork unit, IMapper mapper, IWebHostEnvironment webHostEnvironment, UserManager<User> userManager)
        {
            _unit = unit;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
        }

        public async Task<Response> CreateAsync(ChatMessageDto chatModel)
        {
            if (string.IsNullOrEmpty(chatModel.SenderId) || string.IsNullOrEmpty(chatModel.ReceiverId))
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't save this messages",
                    Success = false
                };
            }

            var senderModel = await _userManager.FindByIdAsync(chatModel.SenderId);
            var receiverModel = await _userManager.FindByIdAsync(chatModel.ReceiverId);
            if (senderModel == null || receiverModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Sender or Receiver isn't exist",
                    Success = false
                };
            }

            var isSuccess = await _unit.ChatMessages.SendMessageAsync(chatModel);
            if (isSuccess == -1)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't save this messages",
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

        public async Task<Response> DeleteAsync(long messageId)
        {
            var chatMessModel = _unit.ChatMessages.GetById(messageId);
            if (chatMessModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any message",
                    Success = false
                };
            }

            if (chatMessModel.FileId.HasValue)
            {
                var fileModel = _unit.ChatFiles.GetById(chatMessModel.FileId.Value);

                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, fileModel.FilePath);

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                _unit.ChatFiles.Remove(fileModel);
            }

            _unit.ChatMessages.Remove(chatMessModel);
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
            var models = _unit.ChatMessages.GetAll();

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<ChatMessageResDto>>(models),
                Success = true
            });
        }

        public Task<Response> GetAsync(long messageId)
        {
            var model = _unit.ChatMessages.GetById(messageId);

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<ChatMessageResDto>(model),
                Success = true
            });
        }

        public async Task<Response> GetMessagesAsync(string senderId, string receiverId)
        {
            var models = await _unit.ChatMessages.GetFullMessageAsync(senderId, receiverId);

            var resultModels = new List<ChatMessageResDto>();

            foreach (var item in models)
            {
                var resultModel = _mapper.Map<ChatMessageResDto>(item);
                resultModel.IsOwnSender = item.SenderId == senderId;

                resultModels.Add(resultModel);
            }

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = resultModels,
                Success = true
            };
        }

        public async Task<Response> GetMembersInClassAsync(string senderId, string classId)
        {
            var enrolls = await _unit.Enrollment
                                     .Include(e => e.User)
                                     .Where(e => e.ClassId == classId && e.UserId != senderId)
                                     .ToListAsync();

            var broadInfoList = new List<ChatBroadItemInfoResDto>();

            foreach (var enroll in enrolls)
            {
                var lastMessage = _unit.ChatMessages
                                       .Include(m => m.ChatFile)
                                       .Where(m => m.ReceiverId == senderId && m.SenderId == enroll.UserId)
                                       .OrderByDescending(m => m.SendAt)
                                       .FirstOrDefault();

                var message = string.Empty;
                if (lastMessage != null && lastMessage.FileId.HasValue)
                {
                    message = lastMessage.ChatFile!.FileName;
                }

                if (lastMessage != null)
                {
                    message = lastMessage.Message;
                }

                var broadInfo = new ChatBroadItemInfoResDto()
                {
                    UserId = enroll.UserId,
                    Image = enroll!.User?.Image?.Replace("\\", "/") ?? "",
                    UserName = enroll!.User?.UserName == null ? enroll!.User!.FirstName + " " + enroll.User!.LastName : enroll!.User.UserName,
                    LastMessage = message ?? "",
                    LastTime = lastMessage == null ? "" : lastMessage.SendAt.ToString("HH:mm"),
                    IsRead = lastMessage?.IsRead ?? false,
                    IsDelete = lastMessage?.IsDelete ?? false,
                };

                broadInfoList.Add(broadInfo);
            }

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = broadInfoList,
                Success = true
            };
        }

        public async Task<Response> GetTeacherInClassAsync(string senderId, string classId)
        {
            var classModel = await _unit.Classes
                                        .Include(c => c.Teacher)
                                        .FirstOrDefaultAsync(c => c.ClassId == classId);
            if (classModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any classes",
                    Success = false
                };
            }

            var lastMessage = _unit.ChatMessages
                                       .Include(m => m.ChatFile)
                                       .Where(m => m.ReceiverId == senderId && m.SenderId == classModel.Teacher.UserId)
                                       .OrderByDescending(m => m.SendAt)
                                       .FirstOrDefault();

            var message = string.Empty;
            if (lastMessage != null && lastMessage.FileId.HasValue)
            {
                message = lastMessage.ChatFile!.FileName;
            }

            if (lastMessage != null)
            {
                message = lastMessage.Message;
            }

            var broadInfo = new ChatBroadItemInfoResDto()
            {
                UserId = classModel.Teacher.UserId,
                Image = classModel.Teacher.Image?.Replace("\\", "/") ?? "",
                UserName = classModel.Teacher?.UserName == null ? classModel.Teacher!.FirstName + " " + classModel.Teacher!.LastName : classModel.Teacher.UserName,
                LastMessage = message ?? "",
                LastTime = lastMessage == null ? "" : lastMessage.SendAt.ToString("HH:mm"),
                IsRead = lastMessage?.IsRead ?? false,
                IsDelete = lastMessage?.IsDelete ?? false,
            };


            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = broadInfo,
                Success = true
            };
        }
    }
}
