using System.Security.Claims;
using AutoMapper;
using EnglishCenter.DataAccess.UnitOfWork;
using EnglishCenter.Presentation.Models.DTOs;
using EnglishCenter.Presentation.Models.ResDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace EnglishCenter.Presentation.Hub
{
    [Authorize]
    public class ChatHub : Hub<IChatClient>
    {
        private static readonly Dictionary<string, string> _connections = new Dictionary<string, string>();
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;

        public ChatHub(IUnitOfWork unit, IMapper mapper)
        {
            _unit = unit;
            _mapper = mapper;
        }

        public override async Task OnConnectedAsync()
        {
            try
            {
                var userID = Context.User!.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;

                if (!_connections.ContainsKey(userID))
                {
                    _connections.Add(userID, Context.ConnectionId);
                }

                await Clients.Others.Online(userID);

                await base.OnConnectedAsync();
            }
            catch (Exception ex)
            {
                await Clients.Caller.ReceiverError(ex.Message);
            }
        }

        public async Task SendMessage(ChatMessageDto message)
        {
            var userId = _connections.FirstOrDefault(x => x.Value == Context.ConnectionId).Key;
            if (string.IsNullOrEmpty(userId))
            {
                userId = Context.User!.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            }

            message.SenderId = userId;

            var messageId = await _unit.ChatMessages.SendMessageAsync(message);
            if (messageId == -1)
            {
                await Clients.Caller.ReceiverError("Can't send messages");
            }
            else
            {
                var messageModel = _unit.ChatMessages
                                        .Include(c => c.ChatFile)
                                        .FirstOrDefault(m => m.MessageId == messageId);

                var messageRes = _mapper.Map<ChatMessageResDto>(messageModel);
                messageRes.IsOwnSender = false;
                await Clients.User(message.ReceiverId).ReceiveMessage(messageRes);

                messageRes.IsOwnSender = true;
                await Clients.Caller.ReceiveMessage(messageRes);
            }
        }

        public async Task ReadMessage(string senderId)
        {
            var userID = Context.User!.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;

            await _unit.ChatMessages.ReadMessageAsync(senderId, userID);

            await _unit.CompleteAsync();

            await Clients.User(senderId).ReadMessageForSender(userID);
        }

        public async Task RemoveMessage(long messageId)
        {
            var messageModel = _unit.ChatMessages.GetById(messageId);

            var removeSuccess = await _unit.ChatMessages.RemoveMessageAsync(messageId);
            if (!removeSuccess)
            {
                await Clients.Caller.ReceiverError("Can't send messages");
            }
            else
            {
                await _unit.CompleteAsync();

                var messageRes = _mapper.Map<ChatMessageResDto>(messageModel);
                messageRes.IsOwnSender = false;
                await Clients.User(messageModel.ReceiverId).RemoveMessage(messageRes);

                messageRes.IsOwnSender = true;
                await Clients.Caller.RemoveMessage(messageRes);
            }

        }

        public Task<List<string>> GetOnlineUsers(string classId)
        {
            var usersInClass = _connections.Keys.Where(k =>
            {
                return _unit.Enrollment.IsExist(e => e.UserId == k && e.ClassId == classId);
            }).ToList();

            return Task.FromResult(usersInClass);
        }
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = _connections.FirstOrDefault(x => x.Value == Context.ConnectionId).Key;
            if (!string.IsNullOrEmpty(userId))
            {
                _connections.Remove(userId);

                await Clients.Others.Offline(userId);
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}
