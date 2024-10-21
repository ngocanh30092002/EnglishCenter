using EnglishCenter.DataAccess.Database;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.IRepositories;
using EnglishCenter.DataAccess.UnitOfWork;
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

            var nextAssignment = await context.Assignments.FindAsync(assignmentId);
            if (nextAssignment == null) return false;

            var quesIdOfModel = await GetQuesIdAsync(model);
            var isSameQues = await IsSameAssignQuesAsync((QuesTypeEnum)model.Type, assignmentId, quesIdOfModel);
            if (isSameQues) return false;

            var otherAssignQues = await context.AssignQues
                                                .Where(s => s.AssignQuesId != model.AssignQuesId && s.AssignmentId == model.AssignmentId)
                                                .OrderBy(s => s.NoNum)
                                                .ToListAsync();

            int i = 1;
            foreach (var item in otherAssignQues)
            {
                item.NoNum = i++;
            }

            var maxNoNum = await context.AssignQues
                                        .Where(s => s.AssignmentId == assignmentId  )
                                        .Select(s => (int?)s.NoNum)
                                        .MaxAsync();

            var expectedTimeQues = await GetTimeQuesAsync(model);
            var currentAssignment = await context.Assignments.FindAsync(model.AssignmentId);
            if (currentAssignment == null) return false;

            model.AssignmentId = assignmentId;
            model.NoNum = maxNoNum.HasValue ? maxNoNum.Value + 1 : 1;

            currentAssignment.ExpectedTime = currentAssignment.ExpectedTime != TimeOnly.MinValue ? TimeOnly.FromTimeSpan(currentAssignment.ExpectedTime - expectedTimeQues) : TimeOnly.MinValue;
            nextAssignment.ExpectedTime = nextAssignment.ExpectedTime.Add(expectedTimeQues.ToTimeSpan());

            return true;
        }

        public async Task<bool> ChangeNoNumAsync(AssignQue model, int noNum)
        {
            if (model == null) return false;
            if (noNum <= 0) return false;

            var sameAssignments = context.AssignQues
                                .Where(s => s.AssignmentId == model.AssignmentId)
                                .OrderBy(s => s.NoNum);

            if (!sameAssignments.Any()) return false;

            var maxNoNum = await sameAssignments.MaxAsync(s => s.NoNum);
            if (noNum > maxNoNum) return false;

            var sameAssignmentList = await sameAssignments.ToListAsync();
            var index = sameAssignmentList.FindIndex(s => s.AssignQuesId == model.AssignQuesId);
            var itemMove = sameAssignmentList.ElementAt(index);

            sameAssignmentList.RemoveAt(index);
            sameAssignmentList.Insert(noNum - 1, itemMove);

            for (int i = 0; i < maxNoNum; i++)
            {
                sameAssignmentList[i].NoNum = i + 1;
            }

            return true;
        }

        public async Task<bool> ChangeQuesAsync(AssignQue model, QuesTypeEnum type, long quesId)
        {
            if (model == null) return false;

            var isExist = await IsExistQuesIdAsync(type, quesId);
            if (!isExist) return false;

            var isSameQues = await IsSameAssignQuesAsync(type, model.AssignQuesId, quesId);
            if (isSameQues) return false;

            var currentAssignment = await context.Assignments.FindAsync(model.AssignmentId);
            if (currentAssignment == null) return false;

            var previousTime = await GetTimeQuesAsync(model);

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
                case QuesTypeEnum.Sentence:
                    model.SentenceQuesId = quesId;
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

            var nextTime = await GetTimeQuesAsync(model);

            currentAssignment.ExpectedTime = currentAssignment.ExpectedTime == TimeOnly.MinValue ? TimeOnly.MinValue : TimeOnly.FromTimeSpan(currentAssignment.ExpectedTime - previousTime);
            currentAssignment.ExpectedTime = currentAssignment.ExpectedTime.Add(nextTime.ToTimeSpan());

            return true;
        }

        public async Task<TimeOnly> GetTimeQuesAsync(AssignQue model)
        {
            if (model == null) return TimeOnly.MinValue;

            var result = await LoadQuestionAsync(model);
            if (!result) return TimeOnly.MinValue;

            var timeResult = TimeOnly.MinValue;

            switch (model.Type)
            {
                case (int) QuesTypeEnum.Image:
                    timeResult = model.QuesImage == null ? timeResult : model.QuesImage.Time;
                    break;
                case (int)QuesTypeEnum.Audio:
                    timeResult = model.QuesAudio == null ? timeResult : model.QuesAudio.Time;
                    break;
                case (int)QuesTypeEnum.Conversation:
                    timeResult = model.QuesConversation == null ? timeResult : model.QuesConversation.Time;
                    break;
                case (int)QuesTypeEnum.Sentence:
                    timeResult = model.QuesSentence == null ? timeResult : model.QuesSentence.Time;
                    break;
                case (int)QuesTypeEnum.Single:
                    timeResult = model.QuesSingle == null ? timeResult : model.QuesSingle.Time;
                    break;
                case (int)QuesTypeEnum.Double:
                    timeResult = model.QuesDouble == null ? timeResult : model.QuesDouble.Time;
                    break;
                case (int)QuesTypeEnum.Triple:
                    timeResult = model.QuesTriple == null ? timeResult : model.QuesTriple.Time;
                    break;
                default:
                    throw new ArgumentException("Invalid Question Type");
            }

            return timeResult;
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
                case QuesTypeEnum.Sentence:
                    isExist = await context.QuesRcSentences.AnyAsync(q => q.QuesId == quesId);
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

        public async Task<bool> IsCorrectAnswerAsync(AssignQue model, string selectedAnswer, long? subId)
        {
            if (model == null) return false;

            var isDone = await LoadQuestionAsync(model);
            if (!isDone) return false;

            try
            {
                switch (model.Type)
                {
                    case (int)QuesTypeEnum.Image:
                        return model.QuesImage!.Answer!.CorrectAnswer == selectedAnswer.ToUpper();

                    case (int)QuesTypeEnum.Audio:
                        return model.QuesAudio!.Answer!.CorrectAnswer == selectedAnswer.ToUpper();

                    case (int)QuesTypeEnum.Conversation:
                        if (!subId.HasValue) return false;
                        var subQueCon = model.QuesConversation!.SubLcConversations.FirstOrDefault(s => s.SubId == subId.Value);
                        return subQueCon!.Answer!.CorrectAnswer == selectedAnswer.ToUpper();

                    case (int)QuesTypeEnum.Sentence:
                        return model.QuesSentence!.Answer!.CorrectAnswer == selectedAnswer.ToUpper();

                    case (int)QuesTypeEnum.Single:
                        if (!subId.HasValue) return false;
                        var subQueSingle = model.QuesSingle!.SubRcSingles.FirstOrDefault(s => s.SubId == subId.Value);
                        return subQueSingle!.Answer!.CorrectAnswer == selectedAnswer.ToUpper();

                    case (int)QuesTypeEnum.Double:
                        if (!subId.HasValue) return false;
                        var subQueDouble = model.QuesDouble!.SubRcDoubles.FirstOrDefault(s => s.SubId == subId.Value);
                        return subQueDouble!.Answer!.CorrectAnswer == selectedAnswer.ToUpper();
                    
                    case (int)QuesTypeEnum.Triple:
                        if (!subId.HasValue) return false;
                        var subQueTriple = model.QuesTriple!.SubRcTriples.FirstOrDefault(s => s.SubId == subId.Value);
                        return subQueTriple!.Answer!.CorrectAnswer == selectedAnswer.ToUpper();
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> IsSameAssignQuesAsync(QuesTypeEnum type, long assignmentId, long quesId)
        {
            bool isExist = false;

            switch (type)
            {
                case QuesTypeEnum.Image:
                    isExist = await context.AssignQues
                                        .AnyAsync(q => q.Type == (int)type &&
                                                       q.ImageQuesId == quesId &&
                                                       q.AssignmentId == assignmentId);
                    break;
                case QuesTypeEnum.Audio:
                    isExist = await context.AssignQues
                                        .AnyAsync(q => q.Type == (int)type &&
                                                       q.AudioQuesId == quesId &&
                                                       q.AssignmentId == assignmentId);
                    break;
                case QuesTypeEnum.Conversation:
                    isExist = await context.AssignQues
                                        .AnyAsync(q => q.Type == (int)type &&
                                                       q.ConversationQuesId == quesId &&
                                                       q.AssignmentId == assignmentId);
                    break;
                case QuesTypeEnum.Sentence:
                    isExist = await context.AssignQues
                                        .AnyAsync(q => q.Type == (int)type &&
                                                       q.SentenceQuesId == quesId &&
                                                       q.AssignmentId == assignmentId);
                    break;
                case QuesTypeEnum.Single:
                    isExist = await context.AssignQues
                                        .AnyAsync(q => q.Type == (int)type &&
                                                       q.SingleQuesId == quesId &&
                                                       q.AssignmentId == assignmentId);
                    break;
                case QuesTypeEnum.Double:
                    isExist = await context.AssignQues
                                        .AnyAsync(q => q.Type == (int)type &&
                                                       q.DoubleQuesId == quesId &&
                                                       q.AssignmentId == assignmentId);
                    break;
                case QuesTypeEnum.Triple:
                    isExist = await context.AssignQues
                                        .AnyAsync(q => q.Type == (int)type &&
                                                       q.TripleQuesId == quesId &&
                                                       q.AssignmentId == assignmentId);
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
                    await context.Entry(model)
                                .Reference(m => m.QuesImage)
                                .Query()
                                .Include(a => a.Answer)
                                .LoadAsync();
                    break;
                case QuesTypeEnum.Audio:
                    await context.Entry(model)
                                .Reference(m => m.QuesAudio)
                                .Query()
                                .Include(a => a.Answer)
                                .LoadAsync();
                    break;
                case QuesTypeEnum.Conversation:
                    await context.Entry(model)
                                .Reference(m => m.QuesConversation)
                                .Query()
                                .Include(a => a.SubLcConversations)
                                .ThenInclude(a => a.Answer)
                                .LoadAsync();
                    break;
                case QuesTypeEnum.Sentence:
                    await context.Entry(model)
                                .Reference(m => m.QuesSentence)
                                .Query()
                                .Include(a => a.Answer)
                                .LoadAsync();
                    break;
                case QuesTypeEnum.Single:
                    await context.Entry(model)
                                .Reference(m => m.QuesSingle)
                                .Query()
                                .Include(a => a.SubRcSingles)
                                .ThenInclude(a => a.Answer)
                                .LoadAsync();
                    break;
                case QuesTypeEnum.Double:
                    await context.Entry(model)
                                .Reference(m => m.QuesDouble)
                                .Query()
                                .Include(a => a.SubRcDoubles)
                                .ThenInclude(a => a.Answer)
                                .LoadAsync();
                    break;
                case QuesTypeEnum.Triple:
                    await context.Entry(model)
                                .Reference(m => m.QuesTriple)
                                .Query()
                                .Include(a => a.SubRcTriples)
                                .ThenInclude(a => a.Answer)
                                .LoadAsync();
                    break;
                default:
                    throw new ArgumentException("Invalid Question Type");
            }

            return true;
        }

        public async Task<bool> LoadQuestionWithoutAnswerAsync(AssignQue model)
        {
            if (model == null) return false;

            switch ((QuesTypeEnum)model.Type)
            {
                case QuesTypeEnum.Image:
                    await context.Entry(model)
                                .Reference(m => m.QuesImage)
                                .LoadAsync();
                    break;
                case QuesTypeEnum.Audio:
                    await context.Entry(model)
                                .Reference(m => m.QuesAudio)
                                .LoadAsync();
                    break;
                case QuesTypeEnum.Conversation:
                    await context.Entry(model)
                                .Reference(m => m.QuesConversation)
                                .Query()
                                .Include(a => a.SubLcConversations)
                                .LoadAsync();
                    break;
                case QuesTypeEnum.Sentence:
                    await context.Entry(model)
                                .Reference(m => m.QuesSentence)
                                .LoadAsync();
                    break;
                case QuesTypeEnum.Single:
                    await context.Entry(model)
                                .Reference(m => m.QuesSingle)
                                .Query()
                                .Include(a => a.SubRcSingles)
                                .LoadAsync();
                    break;
                case QuesTypeEnum.Double:
                    await context.Entry(model)
                                .Reference(m => m.QuesDouble)
                                .Query()
                                .Include(a => a.SubRcDoubles)
                                .LoadAsync();
                    break;
                case QuesTypeEnum.Triple:
                    await context.Entry(model)
                                .Reference(m => m.QuesTriple)
                                .Query()
                                .Include(a => a.SubRcTriples)
                                .LoadAsync();
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

            if(model.NoNum.HasValue && assignModel.NoNum != model.NoNum) 
            {
                var changeResult = await ChangeNoNumAsync(assignModel, model.NoNum.Value);
                if (!changeResult) return false;
            }

            return true;
        }

        public async Task<int> GetNumberByAssignmentAsync(long assignmentId)
        {
            int totalQues = 0;
            var assignQues = await context.AssignQues.Where(a => a.AssignmentId == assignmentId).ToListAsync();

            foreach(var assign in assignQues)
            {
                switch((QuesTypeEnum)assign.Type)
                {
                    case QuesTypeEnum.Conversation:
                        totalQues += context.SubLcConversations.Where(s => s.PreQuesId == assign.ConversationQuesId).Count();
                        break;
                    case QuesTypeEnum.Single:
                        totalQues += context.SubRcSingles.Where(s => s.PreQuesId == assign.SingleQuesId).Count();
                        break;
                    case QuesTypeEnum.Double:
                        totalQues += context.SubRcDoubles.Where(s => s.PreQuesId == assign.DoubleQuesId).Count();
                        break;
                    case QuesTypeEnum.Triple:
                        totalQues += context.SubRcTriples.Where(s => s.PreQuesId == assign.TripleQuesId).Count();
                        break;
                    default:
                        totalQues += 1;
                        break;
                }
            }

            return totalQues;
        }

        public Task<long> GetQuesIdAsync(AssignQue model)
        {
            long quesId;

            switch ((QuesTypeEnum)model.Type)
            {
                case QuesTypeEnum.Image:
                    quesId = model.ImageQuesId!.Value;
                    break;
                case QuesTypeEnum.Audio:
                    quesId = model.AudioQuesId!.Value;
                    break;
                case QuesTypeEnum.Conversation:
                    quesId = model.ConversationQuesId!.Value;
                    break;
                case QuesTypeEnum.Sentence:
                    quesId = model.SentenceQuesId!.Value;
                    break;
                case QuesTypeEnum.Single:
                    quesId = model.SingleQuesId!.Value;
                    break;
                case QuesTypeEnum.Double:
                    quesId = model.DoubleQuesId!.Value;
                    break;
                case QuesTypeEnum.Triple:
                    quesId = model.TripleQuesId!.Value;
                    break;
                default:
                    throw new ArgumentException("Invalid Question Type");
            }

            return Task.FromResult(quesId);
        }
    }
}
