using EnglishCenter.DataAccess.Database;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.DataAccess.Repositories.AssignmentRepositories
{
    public class QuesRcTripleRepository : GenericRepository<QuesRcTriple>, IQuesRcTripleRepository
    {
        public QuesRcTripleRepository(EnglishCenterContext context) : base(context)
        {
        }

        public override QuesRcTriple GetById(long id)
        {
            var model = context.QuesRcTriples
                            .Include(s => s.SubRcTriples)
                            .ThenInclude(s => s.Answer)
                            .FirstOrDefault(q => q.QuesId == id);

            return model;
        }

        public override IEnumerable<QuesRcTriple> GetAll()
        {
            var models = context.QuesRcTriples
                                .Include(s => s.SubRcTriples)
                                .ThenInclude(s => s.Answer)
                                .ToList();

            return models;
        }

        public Task<bool> ChangeImage1Async(QuesRcTriple model, string imageUrl)
        {
            if (model == null) return Task.FromResult(false);

            model.Image1 = imageUrl;

            return Task.FromResult(true);
        }

        public Task<bool> ChangeImage2Async(QuesRcTriple model, string imageUrl)
        {
            if (model == null) return Task.FromResult(false);

            model.Image2 = imageUrl;

            return Task.FromResult(true);
        }

        public Task<bool> ChangeImage3Async(QuesRcTriple model, string imageUrl)
        {
            if (model == null) return Task.FromResult(false);

            model.Image3 = imageUrl;

            return Task.FromResult(true);
        }

        public Task<bool> ChangeQuantityAsync(QuesRcTriple model, int quantity)
        {
            if (model == null) return Task.FromResult(false);
            if (quantity <= 0) return Task.FromResult(false);
            if (model.SubRcTriples.Count > quantity) return Task.FromResult(false);

            model.Quantity = quantity;

            return Task.FromResult(true);
        }

        public Task<bool> ChangeTimeAsync(QuesRcTriple model, TimeOnly time)
        {
            if (model == null) return Task.FromResult(false);

            model.Time = time;

            return Task.FromResult(true);
        }

        public Task<bool> ChangeLevelAsync(QuesRcTriple model, int level)
        {
            if (model == null) return Task.FromResult(false);

            if (level <= 0 || level > 4) return Task.FromResult(false);

            model.Level = level;

            return Task.FromResult(true);
        }
    }
}
