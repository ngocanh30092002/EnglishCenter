using EnglishCenter.DataAccess.Database;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.IRepositories;
using EnglishCenter.Presentation.Global.Enum;
using EnglishCenter.Presentation.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.DataAccess.Repositories.AssignmentRepositories
{
    public class AssignQuesRepository : GenericRepository<AssignQue>, IAssignQuesRepository
    {
        public AssignQuesRepository(EnglishCenterContext context) : base(context)
        {

        }

        public async Task<bool> ChangeAssignmentIdAsync(AssignQue model, long assignmentId)
        {
            if (model == null) return false;

            var isExistAssignment = await context.Assignments.FindAsync(assignmentId);
            if (isExistAssignment == null) return false;

            model.AssignmentId = assignmentId;
            return true;
        }

        public async Task<bool> ChangeQuesAsync(AssignQue model, QuesTypeEnum type, long quesId)
        {
            if (model == null) return false;

            var isExist = await IsExistQuesIdAsync(type, quesId);
            if (!isExist) return false;

            model.ImageQuesId = null;
            model.AudioQuesId = null;
            model.ConversationQuesId = null;
            model.SingleQuesId = null;
            model.DoubleQuesId = null;
            model.TripleQuesId = null;

            switch (type)
            {
                case QuesTypeEnum.Image:
                    model.ImageQuesId = quesId;
                    break;
                case QuesTypeEnum.Audio:
                    model.AudioQuesId = quesId;
                    break;
                case QuesTypeEnum.Conversation:
                    model.ConversationQuesId = quesId;
                    break;
                case QuesTypeEnum.Single:
                    model.SingleQuesId = quesId;
                    break;
                case QuesTypeEnum.Double:
                    model.DoubleQuesId = quesId;
                    break;
                case QuesTypeEnum.Triple:
                    model.TripleQuesId = quesId;
                    break;
                default:
                    throw new ArgumentException("Invalid Question Type");
            }

            model.Type = (int)type;

            return true;
        }

        public async Task<List<AssignQue>?> GetByAssignmentAsync(long assignmentId)
        {
            var models = context.AssignQues
                                .Where(a => a.AssignmentId == assignmentId);

            foreach (var model in models)
            {
                var isLoadSuccess = await LoadQuestionAsync(model);
                if (!isLoadSuccess) return null;
            }

            return await models.ToListAsync();
        }

        public async Task<bool> IsExistQuesIdAsync(QuesTypeEnum type, long quesId)
        {
            bool isExist = false;
            switch (type)
            {
                case QuesTypeEnum.Image:
                    isExist = await context.QuesLcImages.AnyAsync(q => q.QuesId == quesId);
                    break;
                case QuesTypeEnum.Audio:
                    isExist = await context.QuesLcAudios.AnyAsync(q => q.QuesId == quesId);
                    break;
                case QuesTypeEnum.Conversation:
                    isExist = await context.QuesLcConversations.AnyAsync(q => q.QuesId == quesId);
                    break;
                case QuesTypeEnum.Single:
                    isExist = await context.QuesRcSingles.AnyAsync(q => q.QuesId == quesId);
                    break;
                case QuesTypeEnum.Double:
                    isExist = await context.QuesRcDoubles.AnyAsync(q => q.QuesId == quesId);
                    break;
                case QuesTypeEnum.Triple:
                    isExist = await context.QuesRcTriples.AnyAsync(q => q.QuesId == quesId);
                    break;
                default:
                    throw new ArgumentException("Invalid Question Type");
            }

            return isExist;
        }

        public async Task<bool> LoadQuestionAsync(AssignQue model)
        {
            if (model == null) return false;

            switch ((QuesTypeEnum)model.Type)
            {
                case QuesTypeEnum.Image:
                    await context.Entry(model).Reference(m => m.QuesImage).LoadAsync();
                    break;
                case QuesTypeEnum.Audio:
                    await context.Entry(model).Reference(m => m.QuesAudio).LoadAsync();
                    break;
                case QuesTypeEnum.Conversation:
                    await context.Entry(model).Reference(m => m.QuesConversation).LoadAsync();
                    break;
                case QuesTypeEnum.Single:
                    await context.Entry(model).Reference(m => m.QuesSingle).LoadAsync();
                    break;
                case QuesTypeEnum.Double:
                    await context.Entry(model).Reference(m => m.QuesDouble).LoadAsync();
                    break;
                case QuesTypeEnum.Triple:
                    await context.Entry(model).Reference(m => m.QuesTriple).LoadAsync();
                    break;
                default:
                    throw new ArgumentException("Invalid Question Type");
            }

            return true;
        }
        public async Task<bool> UpdateAsync(long id, AssignQueDto model)
        {
            var assignModel = await context.AssignQues.FindAsync(id);
            if (assignModel == null) return false;

            if (assignModel.AssignmentId != model.AssignmentId)
            {
                var changeResult = await ChangeAssignmentIdAsync(assignModel, model.AssignmentId);
                if (!changeResult) return false;
            }

            if (assignModel.Type != model.Type)
            {
                if (!Enum.IsDefined(typeof(QuesTypeEnum), model.Type))
                {
                    return false;
                }

                var changeResult = await ChangeQuesAsync(assignModel, (QuesTypeEnum)model.Type, model.QuesId);

                if (!changeResult) return false;
            }

            return true;
        }
    }
}
