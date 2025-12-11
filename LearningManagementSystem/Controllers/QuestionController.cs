using ComplaignManagementSystem.Presentation.Filters;
using LearningManagementSystem.Bussiness.QuestionHandler;
using LearningManagementSystem.Bussiness.QuizHandler;
using LearningManagementSystem.Data.LMSModels;
using LearningManagementSystem.Data.OtherModels;
using log4net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Reflection;

namespace LearningManagementSystem.Presentation.Controllers
{
    [SessionCheck]
    public class QuestionController : Controller
    {
        private readonly IQuestionService _question;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private static readonly ILog log = LogManager.GetLogger(typeof(TrainingController));

        public QuestionController(IQuestionService questionService, IWebHostEnvironment webHostEnvironment)
        {
            _question = questionService;
            _webHostEnvironment = webHostEnvironment;
        }

        private bool IsUserLoggedIn()
        {
            return !string.IsNullOrEmpty(HttpContext.Session.GetString("UserName"));
        }

        public IActionResult QuestionIndex(int moduleId)
        {
            if (!IsUserLoggedIn())
                return RedirectToAction("Login", "User");

            HttpContext.Session.SetString("moduleId", moduleId.ToString());
            ViewBag.QuestionList = _question.getAllList(moduleId);
            ViewBag.TrainingQuestion_QuizId = new SelectList(_question.getQuizList(moduleId).Result.ToList(), "id", "name");
            ViewBag.ModuleName = _question.GetModuleName(moduleId);
            return View();
        }

        [HttpPost]
        public ActionResult Create(IFormCollection collection)
        {
            var moduleId = HttpContext.Session.GetString("moduleId");
            try
            {
                var UserName = HttpContext.Session.GetString("UserName");
               
                var question = _question.CreateQuestionAnswers(collection);
                TempData["ToastMessage"] = "SubmittedQuestionSuccessfully!";
                log.Info($"Created Question by : {UserName}. Question Record : {question.TrainingQuestionId}");
                return RedirectToAction(nameof(QuestionIndex), new { moduleId = moduleId });
            }
            catch (Exception ex)
            {
                log.Error($"Error : {ex}");
                return RedirectToAction(nameof(QuestionIndex), new { moduleId = moduleId });
            }
        }

        public async Task<IActionResult> Edit(int id) 
        {
            var moduleId = HttpContext.Session.GetString("moduleId");
            // Simulate fetching from database                        
            TrainingQuestion question = _question.getListId(id);
            var answerList = _question.getAnswerList(id);
            var correctAnswer = _question.getCorrectAnswer(answerList, question);
            var model = new QuestionViewModel
            {
                Question = question,
                Answers = answerList,
                CorrectAnswer = correctAnswer
            };
            ViewBag.TrainingQuestion_QuizId = new SelectList(_question.getQuizList(Convert.ToInt32(moduleId)).Result.ToList(), "id", "name", question.TrainingQuestionQuizId);

            return PartialView("_QuestionEditPartial", model);
        }

        [HttpPost]
        public ActionResult Edit(IFormCollection collection)
        {
            var moduleId = HttpContext.Session.GetString("moduleId");
            try
            {
                var UserName = HttpContext.Session.GetString("UserName");

                var question = _question.UpdateQuestionAnswers(collection);
                TempData["ToastMessage"] = "UpdatedQuestionSuccessfully!";
                log.Info($"Created Question by : {UserName}. Question Record : {question.TrainingQuestionId}");
                return RedirectToAction(nameof(QuestionIndex), new { moduleId = moduleId });
            }
            catch (Exception ex)
            {
                log.Error($"Error : {ex}");
                return RedirectToAction(nameof(QuestionIndex), new { moduleId = moduleId });
            }
        }

        public async Task<IActionResult> QuestionDetails(int id)
        {
            var moduleId = HttpContext.Session.GetString("moduleId");
            // Simulate fetching from database                        
            TrainingQuestion question = _question.getListId(id);
            var answerList = _question.getAnswerList(id);

            var correctAnswer = _question.getCorrectAnswer(answerList, question);

            var model = new QuestionViewModel
            {
                Question = question,
                Answers = answerList,
                CorrectAnswer = correctAnswer
            };         
            return PartialView("_QuestionDetailPartial", model);
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            try
            {
                var UserName = HttpContext.Session.GetString("UserName");
                var question = _question.DeleteQuestion(id);
                TempData["ToastMessage"] = "DeletedQuestionSuccessfully!";

                log.Info($"Deleted Question by : {UserName}. Question Record : {question.TrainingQuestionId}");
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                log.Error($"Error : {ex}");
                return Json(new { success = false, message = ex.Message });
            }
        }

        //[HttpPost]
        //public JsonResult Delete(int id)
        //{
        //    try
        //    {
        //        var UserName = HttpContext.Session.GetString("UserName");
        //        var quiz = _quiz.DeleteQuiz(id);
        //        TempData["ToastMessage"] = "DeletedQuizSuccessfully!";

        //        log.Info($"Deleted Quiz by : {UserName}. Quiz Record : {quiz.TrainingQuizId}");
        //        return Json(new { success = true });
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error($"Error : {ex}");
        //        return Json(new { success = false, message = ex.Message });
        //    }
        //}
    }
}
