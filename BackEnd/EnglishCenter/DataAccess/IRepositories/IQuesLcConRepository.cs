using EnglishCenter.DataAccess.Entities;

namespace EnglishCenter.DataAccess.IRepositories
{
    public interface IQuesLcConRepository : IGenericRepository<QuesLcConversation>
    {
        public Task<bool> ChangeTimeAsync(QuesLcConversation model, TimeOnly time);
        public Task<bool> ChangeQuantityAsync(QuesLcConversation model, int quantity);
        public Task<bool> ChangeImageAsync(QuesLcConversation model, string imageUrl);
        public Task<bool> ChangeAudioAsync(QuesLcConversation model, string audioUrl);
    }
}
