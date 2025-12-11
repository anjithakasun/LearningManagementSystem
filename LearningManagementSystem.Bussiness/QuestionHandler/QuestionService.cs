using LearningManagementSystem.Data.LMSModels;
using LearningManagementSystem.Data.OtherModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
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

            // 3. Get which answer is correct
            var correctAnswers = collection.Keys
                                     .Where(k => k.StartsWith("CorrectAnswer") && collection[k] == "on")
                                     .ToList();

            TrainingQuestion question = new TrainingQuestion();
            question.TrainingQuestionQuizId = Convert.ToInt32(quizId);
            question.TrainingQuestionEname = questionEName;
            question.TrainingQuestionSname = questionSName;
            question.TrainingQuestionTname = questionTName;
            question.TrainingQuestionActive = true;
            question.TrainingQuestionCreatedDate = DateTime.Now;
            _db.TrainingQuestions.Add(question);
            _db.SaveChanges();

            // Example: Loop through answers
            for (int i = 0; i < answerENames.Length; i++)
            {
                string eName = answerENames[i];
                string sName = answerSNames[i];
                string tName = answerTNames[i];
                //string asasas = CorrectAnswer[i];

                var questionAnswer = CreateAnswers(eName, sName, tName, question.TrainingQuestionId);
                string key = $"CorrectAnswer_{i}";
                bool isCorrect = collection.ContainsKey(key) && collection[key] == "on";

                if (isCorrect)
                {
                    var questionCorrectAnswers = CreateCorrectAnswer(questionAnswer.TrainingQanswerId, question.TrainingQuestionId);
                }
            }          
            return question;
        }

        public TrainingQuestionAnswer CreateAnswers(string eName, string sName, string tName, int QuestionId)
        {
            var Status = true;
            TrainingQuestionAnswer questionAnswer = new TrainingQuestionAnswer();
            questionAnswer.TrainingQanswerQuestionId = QuestionId;
            questionAnswer.TrainingQanswerEname = eName;
            questionAnswer.TrainingQanswerSname = sName;
            questionAnswer.TrainingQanswerTname = tName;
            questionAnswer.TrainingQanswerActive = true;
            questionAnswer.TrainingQanswerCreatedDate = DateTime.Now;
            _db.TrainingQuestionAnswers.Add(questionAnswer);
            _db.SaveChanges();
            return questionAnswer;
        }

        public TrainingQuestionAnswer UpdateAnswers(int AnswerId, string eName, string sName, string tName)
        {
            var Status = true;
            TrainingQuestionAnswer questionAnswer = _db.TrainingQuestionAnswers.Where(a => a.TrainingQanswerId == AnswerId).FirstOrDefault();
            questionAnswer.TrainingQanswerEname = eName;
            questionAnswer.TrainingQanswerSname = sName;
            questionAnswer.TrainingQanswerTname = tName;
            _db.SaveChanges();
            return questionAnswer;
        }

        public bool deleteAnswers(List<TrainingQuestionAnswer> QuestionAnswers)
        {
            var Status = true;
            foreach(var item in QuestionAnswers)
            {
                item.TrainingQanswerActive = false;
                _db.SaveChanges();
            }
            return Status;
        }

        public TrainingQuestionCorrectAnswer CreateCorrectAnswer(int AnswerId, int QuestionId)
        {
            TrainingQuestionCorrectAnswer questionCorrectAnswer = new TrainingQuestionCorrectAnswer();
            questionCorrectAnswer.TrainingQcorrectAnswerQuestionId = QuestionId;
            questionCorrectAnswer.TrainingQcorrectAnswerQanswerId = AnswerId;
            questionCorrectAnswer.TrainingQcorrectAnswerActive = true;
            questionCorrectAnswer.TrainingQcorrectAnswerCreatedDate = DateTime.Now;
            _db.TrainingQuestionCorrectAnswers.Add(questionCorrectAnswer);
            _db.SaveChanges();
            return questionCorrectAnswer;
        }

        public TrainingQuestionCorrectAnswer UpdateCorrectAnswer(int AnswerId, int QuestionId)
        {
            TrainingQuestionCorrectAnswer questionCorrectAnswer = _db.TrainingQuestionCorrectAnswers.Where(a => a.TrainingQcorrectAnswerQuestionId == QuestionId).FirstOrDefault();
            questionCorrectAnswer.TrainingQcorrectAnswerQanswerId = AnswerId;
            _db.SaveChanges();
            return questionCorrectAnswer;
        }

        public TrainingQuestion getListId(int id)
        {
            var questionDetails = _db.TrainingQuestions.Include(t => t.TrainingQuestionQuiz).Include(t => t.TrainingQuestionQuiz.TrainingQuizModule).Where(a => a.TrainingQuestionId == id).FirstOrDefault();
            return questionDetails;
        }

        public List<TrainingQuestionAnswer> getAnswerList(int id)
        {
            var QuestionAnswerLists = _db.TrainingQuestionAnswers.Include(t => t.TrainingQanswerQuestion).Where(a => a.TrainingQanswerActive == true && a.TrainingQanswerQuestionId == id).ToList();
            return QuestionAnswerLists;
        }

        public List<TrainingQuestionCorrectAnswer> getCorrectAnswer(List<TrainingQuestionAnswer> answerList, TrainingQuestion question)
        {
            var QuestionCorrectAsnwerList = _db.TrainingQuestionCorrectAnswers.Include(t => t.TrainingQcorrectAnswerQanswer).Include(t => t.TrainingQcorrectAnswerQuestion).Where(a => a.TrainingQcorrectAnswerActive == true && a.TrainingQcorrectAnswerQuestionId == question.TrainingQuestionId && answerList.Select(a => a.TrainingQanswerId).Contains(a.TrainingQcorrectAnswerQanswerId.Value)).ToList();
            return QuestionCorrectAsnwerList;
        }

        public TrainingQuestion UpdateQuestionAnswers(IFormCollection collection)
        {
            string QuestionId = collection["QuestionId"];
            string quizId = collection["TrainingQuestion_EQuizId"];
            string questionEName = collection["TrainingQuestion_EEName"];
            string questionSName = collection["TrainingQuestion_ESName"];
            string questionTName = collection["TrainingQuestion_ETName"];

            // 2. Get all answer fields
            var AnswerId = collection["AnswerId"].ToArray();
            var answerENames = collection["TrainingQAnswer_EEName"].ToArray();
            var answerSNames = collection["TrainingQAnswer_ESName"].ToArray();
            var answerTNames = collection["TrainingQAnswer_ETName"].ToArray();

            var answerIdsInt = AnswerId.Where(a => !string.IsNullOrEmpty(a)).Select(a => int.Parse(a!)).ToArray();
            var existAnswers = _db.TrainingQuestionAnswers.Where(a => a.TrainingQanswerActive == true && !answerIdsInt.Contains(a.TrainingQanswerId)).ToList();

            if (existAnswers.Count() != 0)
            {
                deleteAnswers(existAnswers);
            }

            // 3. Get which answer is correct
            var correctAnswers = collection.Keys
                                     .Where(k => k.StartsWith("CorrectAnswer") && collection[k] == "on")
                                     .ToList();

            TrainingQuestion question = _db.TrainingQuestions.Where(a => a.TrainingQuestionId == Convert.ToInt32(QuestionId)).FirstOrDefault();
            question.TrainingQuestionQuizId = Convert.ToInt32(quizId);
            question.TrainingQuestionEname = questionEName;
            question.TrainingQuestionSname = questionSName;
            question.TrainingQuestionTname = questionTName;
            _db.SaveChanges();

            // Example: Loop through answers
            for (int i = 0; i < answerENames.Length; i++)
            {
                string eName = answerENames[i];
                string sName = answerSNames[i];
                string tName = answerTNames[i];
                string answerId = AnswerId?.ElementAtOrDefault(i);

                TrainingQuestionAnswer questionAnswer = new TrainingQuestionAnswer();
                if (answerId != null)
                {
                    questionAnswer = UpdateAnswers(Convert.ToInt32(answerId), eName, sName, tName);
                }
                else
                {
                    questionAnswer = CreateAnswers(eName, sName, tName, question.TrainingQuestionId);
                }

                string key = $"CorrectAnswerE_{i}";
                bool isCorrect = collection.ContainsKey(key);
                if (isCorrect)
                {
                    var existingOrNot = _db.TrainingQuestionCorrectAnswers.Where(a => a.TrainingQcorrectAnswerQanswerId == questionAnswer.TrainingQanswerId && a.TrainingQcorrectAnswerQuestionId == question.TrainingQuestionId).Count();
                    if(existingOrNot == 0)
                    {
                        var questionCorrectAnswers = UpdateCorrectAnswer(questionAnswer.TrainingQanswerId, question.TrainingQuestionId);
                    }
                }
            }
            return question;
        }

        public TrainingQuestion DeleteQuestion(int id)
        {
            TrainingQuestion question = _db.TrainingQuestions.Where(a => a.TrainingQuestionId == id).FirstOrDefault();
            question.TrainingQuestionActive = false;
            _db.SaveChanges();
            return question;
        }
    }
}
