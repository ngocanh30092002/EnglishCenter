using AutoMapper;
using EnglishCenter.DataAccess.Database;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.IRepositories;
using EnglishCenter.Presentation.Global.Enum;
using EnglishCenter.Presentation.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.DataAccess.Repositories.HomeworkRepositories
{
    public class HomeQuesRepository : GenericRepository<HomeQue>, IHomeQuesRepository
    {
        private readonly IMapper _mapper;

        public HomeQuesRepository(EnglishCenterContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }

        public async Task<bool> ChangeHomeworkIdAsync(HomeQue model, long homeworkId)
        {
            if (model == null) return false;

            var nextHomework = await context.Homework.FindAsync(homeworkId);
            if (nextHomework == null) return false;

            var quesIdOfModel = await GetQuesIdAsync(model);

            var isSameQues = await IsSameHomeQuesAsync((QuesTypeEnum)model.Type, homeworkId, quesIdOfModel);
            if (isSameQues) return false;

            var otherHomeQues = await context.HomeQues
                                            .Where(s => s.HomeQuesId != model.HomeQuesId && s.HomeworkId == model.HomeworkId)
                                            .OrderBy(s => s.NoNum)
                                            .ToListAsync();

            int i = 1;
            foreach (var item in otherHomeQues)
            {
                item.NoNum = i++;
            }

            var maxNoNum = await context.HomeQues
                                        .Where(s => s.HomeworkId == homeworkId)
                                        .Select(s => (int?)s.NoNum)
                                        .MaxAsync();
            var expectedTimeQues = await GetTimeQuesAsync(model);
            var currentHomework = await context.Homework.FindAsync(model.HomeworkId);
            if (currentHomework == null) return false;

            model.HomeworkId = homeworkId;
            model.NoNum = maxNoNum.HasValue ? maxNoNum.Value + 1 : 1;

            currentHomework.ExpectedTime = currentHomework.ExpectedTime != TimeOnly.MinValue ? TimeOnly.FromTimeSpan(currentHomework.ExpectedTime - expectedTimeQues) : TimeOnly.MinValue;
            nextHomework.ExpectedTime = nextHomework.ExpectedTime.Add(expectedTimeQues.ToTimeSpan());

            return true;
        }

        public async Task<bool> ChangeNoNumAsync(HomeQue model, int noNum)
        {
            if (model == null) return false;
            if (noNum <= 0) return false;

            var sameHomework = context.HomeQues
                                .Where(s => s.HomeworkId == model.HomeworkId)
                                .OrderBy(s => s.NoNum);

            if (!sameHomework.Any()) return false;

            var maxNoNum = await sameHomework.MaxAsync(s => s.NoNum);
            if (noNum > maxNoNum) return false;

            var sameHomeworkList = await sameHomework.ToListAsync();
            var index = sameHomeworkList.FindIndex(s => s.HomeQuesId == model.HomeQuesId);
            var itemMove = sameHomeworkList.ElementAt(index);

            sameHomeworkList.RemoveAt(index);
            sameHomeworkList.Insert(noNum - 1, itemMove);

            for (int i = 0; i < maxNoNum; i++)
            {
                sameHomeworkList[i].NoNum = i + 1;
            }

            return true;
        }

        public async Task<bool> ChangeQuesAsync(HomeQue model, QuesTypeEnum type, long quesId)
        {
            if (model == null) return false;

            var isExist = await IsExistQuesIdAsync(type, quesId);
            if (!isExist) return false;

            var isSameQues = await IsSameHomeQuesAsync(type, model.HomeworkId, quesId);
            if (isSameQues) return false;

            var currentHomework = await context.Homework.FindAsync(model.HomeworkId);
            if (currentHomework == null) return false;

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

            currentHomework.ExpectedTime = currentHomework.ExpectedTime == TimeOnly.MinValue ? TimeOnly.MinValue : TimeOnly.FromTimeSpan(currentHomework.ExpectedTime - previousTime);
            currentHomework.ExpectedTime = currentHomework.ExpectedTime.Add(nextTime.ToTimeSpan());

            return true;
        }

        public async Task<List<HomeQue>?> GetByHomeworkAsync(long homeworkId)
        {
            var models = context.HomeQues
                                .Where(a => a.HomeworkId == homeworkId);

            foreach (var model in models)
            {
                var isLoadSuccess = await LoadQuestionAsync(model);
                if (!isLoadSuccess) return null;
            }

            return await models.ToListAsync();
        }

        public async Task<int> GetNumberByHomeworkAsync(long homeworkId)
        {
            int totalQues = 0;
            var homeQues = await context.HomeQues.Where(a => a.HomeworkId == homeworkId).ToListAsync();

            foreach (var assign in homeQues)
            {
                switch ((QuesTypeEnum)assign.Type)
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

        public Task<long> GetQuesIdAsync(HomeQue model)
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

        public async Task<TimeOnly> GetTimeQuesAsync(HomeQue model)
        {
            if (model == null) return TimeOnly.MinValue;

            var result = await LoadQuestionAsync(model);
            if (!result) return TimeOnly.MinValue;

            var timeResult = TimeOnly.MinValue;

            switch (model.Type)
            {
                case (int)QuesTypeEnum.Image:
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

        public async Task<object> GetAnswerInfoAsync(long homeQuesId, long? subId)
        {
            var homeQueModel = await context.HomeQues.FindAsync(homeQuesId);
            if (homeQueModel == null) return null;

            await LoadQuestionAsync(homeQueModel);

            switch (homeQueModel.Type)
            {
                case (int)QuesTypeEnum.Image:
                    return _mapper.Map<AnswerLcImageDto>(homeQueModel.QuesImage.Answer);
                case (int)QuesTypeEnum.Audio:
                    return _mapper.Map<AnswerLcAudioDto>(homeQueModel.QuesAudio.Answer);
                case (int)QuesTypeEnum.Conversation:
                    return _mapper.Map<AnswerLcConDto>(homeQueModel.QuesConversation.SubLcConversations.FirstOrDefault(s => s.SubId == subId.Value).Answer);
                case (int)QuesTypeEnum.Sentence:
                    return _mapper.Map<AnswerRcSentenceDto>(homeQueModel.QuesSentence.Answer);
                case (int)QuesTypeEnum.Single:
                    return _mapper.Map<AnswerRcSingleDto>(homeQueModel.QuesSingle.SubRcSingles.FirstOrDefault(s => s.SubId == subId.Value).Answer);
                case (int)QuesTypeEnum.Double:
                    return _mapper.Map<AnswerRcDoubleDto>(homeQueModel.QuesDouble.SubRcDoubles.FirstOrDefault(s => s.SubId == subId.Value).Answer);
                case (int)QuesTypeEnum.Triple:
                    return _mapper.Map<AnswerRcTripleDto>(homeQueModel.QuesTriple.SubRcTriples.FirstOrDefault(s => s.SubId == subId.Value).Answer);
            }

            return null;
        }

        public async Task<bool> IsCorrectAnswerAsync(HomeQue model, string selectedAnswer, long? subId)
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

        public async Task<bool> IsSameHomeQuesAsync(QuesTypeEnum type, long homeworkId, long quesId)
        {
            bool isExist = false;

            switch (type)
            {
                case QuesTypeEnum.Image:
                    isExist = await context.HomeQues
                                        .AnyAsync(q => q.Type == (int)type &&
                                                       q.ImageQuesId == quesId &&
                                                       q.HomeworkId == homeworkId);
                    break;
                case QuesTypeEnum.Audio:
                    isExist = await context.HomeQues
                                        .AnyAsync(q => q.Type == (int)type &&
                                                       q.AudioQuesId == quesId &&
                                                       q.HomeworkId == homeworkId);
                    break;
                case QuesTypeEnum.Conversation:
                    isExist = await context.HomeQues
                                        .AnyAsync(q => q.Type == (int)type &&
                                                       q.ConversationQuesId == quesId &&
                                                       q.HomeworkId == homeworkId);
                    break;
                case QuesTypeEnum.Sentence:
                    isExist = await context.HomeQues
                                        .AnyAsync(q => q.Type == (int)type &&
                                                       q.SentenceQuesId == quesId &&
                                                       q.HomeworkId == homeworkId);
                    break;
                case QuesTypeEnum.Single:
                    isExist = await context.HomeQues
                                        .AnyAsync(q => q.Type == (int)type &&
                                                       q.SingleQuesId == quesId &&
                                                       q.HomeworkId == homeworkId);
                    break;
                case QuesTypeEnum.Double:
                    isExist = await context.HomeQues
                                        .AnyAsync(q => q.Type == (int)type &&
                                                       q.DoubleQuesId == quesId &&
                                                       q.HomeworkId == homeworkId);
                    break;
                case QuesTypeEnum.Triple:
                    isExist = await context.HomeQues
                                        .AnyAsync(q => q.Type == (int)type &&
                                                       q.TripleQuesId == quesId &&
                                                       q.HomeworkId == homeworkId);
                    break;
                default:
                    throw new ArgumentException("Invalid Question Type");
            }

            return isExist;
        }

        public async Task<bool> LoadQuestionAsync(HomeQue model)
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

        public async Task<bool> LoadQuestionWithAnswerAsync(HomeQue model)
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

        public async Task<bool> UpdateAsync(long id, HomeQueDto model)
        {
            var homeQuesModel = await context.HomeQues.FindAsync(id);
            if (homeQuesModel == null) return false;

            if (homeQuesModel.HomeworkId != model.HomeworkId)
            {
                var changeResult = await ChangeHomeworkIdAsync(homeQuesModel, model.HomeworkId);
                if (!changeResult) return false;
            }

            if (homeQuesModel.Type != model.Type)
            {
                if (!Enum.IsDefined(typeof(QuesTypeEnum), model.Type))
                {
                    return false;
                }

                var changeResult = await ChangeQuesAsync(homeQuesModel, (QuesTypeEnum)model.Type, model.QuesId);

                if (!changeResult) return false;
            }

            if (model.NoNum.HasValue && homeQuesModel.NoNum != model.NoNum)
            {
                var changeResult = await ChangeNoNumAsync(homeQuesModel, model.NoNum.Value);
                if (!changeResult) return false;
            }

            return true;
        }
    }
}
