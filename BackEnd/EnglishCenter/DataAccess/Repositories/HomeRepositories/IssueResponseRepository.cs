using EnglishCenter.DataAccess.Database;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.IRepositories;

namespace EnglishCenter.DataAccess.Repositories.HomeRepositories
{
    public class IssueResponseRepository : GenericRepository<IssueResponse>, IIssueResponseRepository
    {
        public IssueResponseRepository(EnglishCenterContext context) : base(context)
        {
        }

        public async Task<bool> ChangeIssueAsync(IssueResponse issueResponse, long issueId)
        {
            if (issueResponse == null) return false;

            var issueReport = await context.IssueReports.FindAsync(issueId);
            if (issueReport == null) return false;

            issueResponse.IssueId = issueId;

            return true;
        }

        public Task<bool> ChangeMessageAsync(IssueResponse issueResponse, string message)
        {
            if (issueResponse == null) return Task.FromResult(false);

            issueResponse.Message = message;

            return Task.FromResult(true);
        }
    }
}
