using EnglishCenter.DataAccess.Entities;

namespace EnglishCenter.DataAccess.IRepositories
{
    public interface IIssueResponseRepository : IGenericRepository<IssueResponse>
    {
        public Task<bool> ChangeIssueAsync(IssueResponse issueResponse, long issueId);
        public Task<bool> ChangeMessageAsync(IssueResponse issueResponse, string message);
    }
}
