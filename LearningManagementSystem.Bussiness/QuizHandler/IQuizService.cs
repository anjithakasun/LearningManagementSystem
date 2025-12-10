using LearningManagementSystem.Data.LMSModels;
using LearningManagementSystem.Data.OtherModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Bussiness.QuizHandler
{
    public interface IQuizService
    {
        public Task<List<ModuleDto>> getModuleList(int courseId);
        public List<TrainingQuiz> getAllList(int courseId);
        public String GetCourseName(int id);
        public TrainingQuiz CreateQuiz(IFormCollection collection);
        public TrainingQuiz getListId(int id);
        public TrainingQuiz updateQuiz(IFormCollection collection);
        public TrainingQuiz DeleteQuiz(int id);
    }
}
