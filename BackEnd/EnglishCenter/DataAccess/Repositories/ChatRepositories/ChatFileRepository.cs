using EnglishCenter.DataAccess.Database;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.IRepositories;

namespace EnglishCenter.DataAccess.Repositories.ChatRepositories
{
    public class ChatFileRepository : GenericRepository<ChatFile>, IChatFileRepository
    {
        public ChatFileRepository(EnglishCenterContext context) : base(context)
        {
        }
    }
}
