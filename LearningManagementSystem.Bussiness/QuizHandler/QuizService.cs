using LearningManagementSystem.Data.LMSModels;
using LearningManagementSystem.Data.OtherModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Bussiness.QuizHandler
{
    public class QuizService : IQuizService
    {
        private readonly LearningManagementContext _db;

        public QuizService(LearningManagementContext context)
        {
            _db = context;
        }

        public List<TrainingQuiz> getAllList(int courseId)
        {
            var TrainingList = _db.TrainingQuizzes.Include(t => t.TrainingQuizModule).Include(t => t.TrainingQuizModule.TrainingCourseModuleTrainingCourse).Where(a => a.TrainingQuizActive == true && a.TrainingQuizModule.TrainingCourseModuleTrainingCourseId == courseId).ToList();
            return TrainingList;
        }

        public async Task<List<ModuleDto>> getModuleList(int courseId)
        {
            var list = _db.TrainingCourseModules.Where(a => a.TrainingCourseModuleActive == true && a.TrainingCourseModuleTrainingCourseId == courseId)
                        .Select(t => new ModuleDto
                        {
                            id = t.TrainingCourseModuleId,
                            name = t.TrainingCourseModuleTrainingCourse.TrainingCourseEname + " | " + t.TrainingCourseModuleEname
                        })
                        .ToList();
            return list;
        }

        public string GetCourseName(int id)
        {
            var CourseName = _db.TrainingCourses.Where(a => a.TrainingCourseActive == true && a.TrainingCourseId == id).Select(a => a.TrainingCourseTraining.TrainingEname + " | " + a.TrainingCourseEname).FirstOrDefault();
            return CourseName;
        }

        public TrainingQuiz CreateQuiz(IFormCollection collection)
        {
            var TrainingQuiz_ModuleId = collection["TrainingQuiz_ModuleId"].ToString();
            var TrainingQuiz_EName = collection["TrainingQuiz_EName"].ToString();
            var TrainingQuiz_SName = collection["TrainingQuiz_SName"].ToString();
            var TrainingQuiz_TName = collection["TrainingQuiz_TName"].ToString();
            var TrainingQuiz_NoOfAnswerRequired = collection["TrainingQuiz_NoOfAnswerRequired"].ToString();
            var TrainingQuiz_AttemptCount = collection["TrainingQuiz_AttemptCount"].ToString();

            TrainingQuiz quiz = new TrainingQuiz();
            quiz.TrainingQuizModuleId = Convert.ToInt32(TrainingQuiz_ModuleId);
            quiz.TrainingQuizEname = TrainingQuiz_EName;
            quiz.TrainingQuizSname = TrainingQuiz_SName;
            quiz.TrainingQuizTname = TrainingQuiz_TName;
            quiz.TrainingQuizNoOfAnswerRequired = Convert.ToInt16(TrainingQuiz_NoOfAnswerRequired); ;
            quiz.TrainingQuizAttemptCount = Convert.ToInt16(TrainingQuiz_AttemptCount);
            quiz.TrainingQuizActive = true;
            quiz.TrainingQuizCreatedDate = System.DateTime.Now;
            _db.TrainingQuizzes.Add(quiz);
            _db.SaveChanges();
            return quiz;
        }

        public TrainingQuiz getListId(int id)
        {
            var quizDetails = _db.TrainingQuizzes.Include(t => t.TrainingQuizModule).Include(t => t.TrainingQuizModule.TrainingCourseModuleTrainingCourse).Where(a => a.TrainingQuizId == id).FirstOrDefault();
            return quizDetails;
        }

        public TrainingQuiz updateQuiz(IFormCollection collection)
        {
            var TrainingQuiz_Id = collection["Id"].ToString();
            var TrainingQuiz_ModuleId = collection["TrainingQuiz_EModuleId"].ToString();
            var TrainingQuiz_EName = collection["TrainingQuiz_EEName"].ToString();
            var TrainingQuiz_SName = collection["TrainingQuiz_ESName"].ToString();
            var TrainingQuiz_TName = collection["TrainingQuiz_ETName"].ToString();
            var TrainingQuiz_NoOfAnswerRequired = collection["TrainingQuiz_ENoOfAnswerRequired"].ToString();
            var TrainingQuiz_AttemptCount = collection["TrainingQuiz_EAttemptCount"].ToString();

            TrainingQuiz quiz = _db.TrainingQuizzes.Where(a => a.TrainingQuizId == Convert.ToInt32(TrainingQuiz_Id)).FirstOrDefault();
            quiz.TrainingQuizModuleId = Convert.ToInt32(TrainingQuiz_ModuleId);
            quiz.TrainingQuizEname = TrainingQuiz_EName;
            quiz.TrainingQuizSname = TrainingQuiz_SName;
            quiz.TrainingQuizTname = TrainingQuiz_TName;
            quiz.TrainingQuizNoOfAnswerRequired = Convert.ToInt16(TrainingQuiz_NoOfAnswerRequired); ;
            quiz.TrainingQuizAttemptCount = Convert.ToInt16(TrainingQuiz_AttemptCount);
            _db.SaveChanges();
            return quiz;
        }

        public TrainingQuiz DeleteQuiz(int id)
        {
            TrainingQuiz quiz = _db.TrainingQuizzes.Where(a => a.TrainingQuizId == id).FirstOrDefault();
            quiz.TrainingQuizActive = false;
            _db.SaveChanges();
            return quiz;
        }
    }
}
