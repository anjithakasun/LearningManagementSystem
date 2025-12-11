using LearningManagementSystem.Data.LMSModels;
using LearningManagementSystem.Data.OtherModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Bussiness.QuestionHandler
{
    public interface IQuestionService
    {
        public Task<List<QuizDto>> getQuizList(int moduleId);
        public List<TrainingQuestion> getAllList(int moduleId);
        public String GetModuleName(int id);
        public TrainingQuestion CreateQuestionAnswers(IFormCollection collection);
        public TrainingQuestion getListId(int id);
        public List<TrainingQuestionAnswer> getAnswerList(int id);
        public List<TrainingQuestionCorrectAnswer> getCorrectAnswer(List<TrainingQuestionAnswer> answerList, TrainingQuestion question);
        public TrainingQuestion UpdateQuestionAnswers(IFormCollection collection);
        public TrainingQuestion DeleteQuestion(int id);
    }
}
