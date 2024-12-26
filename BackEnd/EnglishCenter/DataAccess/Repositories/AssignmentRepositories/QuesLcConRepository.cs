using EnglishCenter.DataAccess.Database;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.DataAccess.Repositories.AssignmentRepositories
{
    public class QuesLcConRepository : GenericRepository<QuesLcConversation>, IQuesLcConRepository
    {
        public QuesLcConRepository(EnglishCenterContext context) : base(context)
        {

        }
        public override QuesLcConversation GetById(long id)
        {
            var model = context.QuesLcConversations
                            .Include(s => s.SubLcConversations)
                            .ThenInclude(s => s.Answer)
                            .FirstOrDefault(q => q.QuesId == id);

            return model;
        }

        public override IEnumerable<QuesLcConversation> GetAll()
        {
            var models = context.QuesLcConversations
                                .Include(s => s.SubLcConversations)
                                .ThenInclude(s => s.Answer)
                                .ToList();

            return models;
        }

        public Task<bool> ChangeAudioAsync(QuesLcConversation model, string audioUrl)
        {
            if (model == null) return Task.FromResult(false);

            model.Audio = audioUrl;
            return Task.FromResult(true);
        }

        public Task<bool> ChangeImageAsync(QuesLcConversation model, string imageUrl)
        {
            if (model == null) return Task.FromResult(false);

            model.Image = imageUrl;
            return Task.FromResult(true);
        }

        public Task<bool> ChangeQuantityAsync(QuesLcConversation model, int quantity)
        {
            if (model == null) return Task.FromResult(false);
            if (quantity <= 0) return Task.FromResult(false);
            if (model.SubLcConversations.Count > quantity) return Task.FromResult(false);

            model.Quantity = quantity;
            return Task.FromResult(true);
        }

        public Task<bool> ChangeTimeAsync(QuesLcConversation model, TimeOnly time)
        {
            if (model == null) return Task.FromResult(false);

            model.Time = time;
            return Task.FromResult(true);
        }

        public Task<bool> ChangeLevelAsync(QuesLcConversation model, int level)
        {
            if (model == null) return Task.FromResult(false);

            if (level <= 0 || level > 4) return Task.FromResult(false);

            model.Level = level;

            return Task.FromResult(true);
        }
    }
}
