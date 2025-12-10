using ComplaignManagementSystem.Presentation.Filters;
using LearningManagementSystem.Bussiness.QuestionHandler;
using LearningManagementSystem.Bussiness.QuizHandler;
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
               
                var Course = _question.CreateQuestionAnswers(collection);
                TempData["ToastMessage"] = "SubmittedCourseSuccessfully!";
                //log.Info($"Created Course by : {UserName}. Course Record : {Course.TrainingCourseId}");
                return RedirectToAction(nameof(QuestionIndex), new { moduleId = moduleId });
            }
            catch (Exception ex)
            {
                log.Error($"Error : {ex}");
                return RedirectToAction(nameof(QuestionIndex), new { moduleId = moduleId });
            }
        }

    }
}
