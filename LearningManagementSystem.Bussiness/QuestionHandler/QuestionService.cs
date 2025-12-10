using LearningManagementSystem.Data.LMSModels;
using LearningManagementSystem.Data.OtherModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Bussiness.QuestionHandler
{
    public class QuestionService : IQuestionService
    {
        private readonly LearningManagementContext _db;

        public QuestionService(LearningManagementContext context)
        {
            _db = context;
        }


        public List<TrainingQuestion> getAllList(int moduleId)
        {
            var QuestionLists = _db.TrainingQuestions.Include(t => t.TrainingQuestionQuiz).Include(t => t.TrainingQuestionQuiz.TrainingQuizModule).Where(a => a.TrainingQuestionActive == true && a.TrainingQuestionQuiz.TrainingQuizModuleId == moduleId).ToList();
            return QuestionLists;
        }

        public string GetModuleName(int id)
        {
            var ModuleName = _db.TrainingCourseModules.Where(a => a.TrainingCourseModuleActive == true && a.TrainingCourseModuleId == id).Select(a => a.TrainingCourseModuleTrainingCourse.TrainingCourseEname + " | " + a.TrainingCourseModuleEname).FirstOrDefault();
            return ModuleName;
        }

        public async Task<List<QuizDto>> getQuizList(int moduleId)
        {
            var list = _db.TrainingQuizzes.Where(a => a.TrainingQuizActive == true && a.TrainingQuizModuleId == moduleId)
                        .Select(t => new QuizDto
                        {
                            id = t.TrainingQuizId,
                            name = t.TrainingQuizModule.TrainingCourseModuleEname + " | " + t.TrainingQuizEname
                        })
                        .ToList();
            return list;
        }

        public TrainingQuestion CreateQuestionAnswers(IFormCollection collection)
        {
            string quizId = collection["TrainingQuestion_QuizId"];
            string questionEName = collection["TrainingQuestion_EName"];
            string questionSName = collection["TrainingQuestion_SName"];
            string questionTName = collection["TrainingQuestion_TName"];

            // 2. Get all answer fields
            var answerENames = collection["TrainingQAnswer_EName"].ToArray();
            var answerSNames = collection["TrainingQAnswer_SName"].ToArray();
            var answerTNames = collection["TrainingQAnswer_TName"].ToArray();
            var CorrectAnswer = collection["CorrectAnswer"].ToArray();

            // 3. Get which answer is correct
            var correctAnswers = collection.Keys
                                     .Where(k => k.StartsWith("CorrectAnswer") && collection[k] == "on")
                                     .ToList();

            // Example: Loop through answers
            for (int i = 0; i < answerENames.Length; i++)
            {
                string eName = answerENames[i];
                string sName = answerSNames[i];
                string tName = answerTNames[i];
                //string asasas = CorrectAnswer[i];

                string key = $"CorrectAnswer_{i}";
                bool isCorrect = collection.ContainsKey(key) && collection[key] == "on";
                // Check if this answer is marked as correct
                //bool isCorrectss = correctAnswers.Any(c => c == $"CorrectAnswer_{i}");

                // TODO: Save to database or further processing
                Console.WriteLine($"Answer {i + 1}: EN={eName}, SI={sName}, TA={tName}, Correct={isCorrect}");
            }



            TrainingQuestion quetsion = new TrainingQuestion();


            return quetsion;
        }



    }
}
