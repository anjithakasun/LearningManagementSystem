using ComplaignManagementSystem.Presentation.Filters;
using LearningManagementSystem.Bussiness.CourseHandler;
using LearningManagementSystem.Bussiness.QuizHandler;
using LearningManagementSystem.Data.LMSModels;
using log4net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LearningManagementSystem.Presentation.Controllers
{
    [SessionCheck]
    public class QuizController : Controller
    {
        private readonly IQuizService _quiz;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private static readonly ILog log = LogManager.GetLogger(typeof(TrainingController));

        public QuizController(IQuizService quizService, IWebHostEnvironment webHostEnvironment)
        {
            _quiz = quizService;
            _webHostEnvironment = webHostEnvironment;
        }

        private bool IsUserLoggedIn()
        {
            return !string.IsNullOrEmpty(HttpContext.Session.GetString("UserName"));
        }

        [HttpGet]
        public IActionResult Index(int courseId)
        {
            if (!IsUserLoggedIn())
                return RedirectToAction("Login", "User");
            HttpContext.Session.SetString("courseId", courseId.ToString());
            ViewBag.QuizList = _quiz.getAllList(courseId);
            ViewBag.TrainingQuiz_ModuleId = new SelectList(_quiz.getModuleList(courseId).Result.ToList(), "id", "name");
            ViewBag.CourseName = _quiz.GetCourseName(courseId);
            return View();
        }

        [HttpPost]
        public ActionResult Create(IFormCollection collection)
        {
            var courseId = HttpContext.Session.GetString("courseId");
            try
            {
                var UserName = HttpContext.Session.GetString("UserName");
                var Quiz = _quiz.CreateQuiz(collection);
                TempData["ToastMessage"] = "SubmittedQuizSuccessfully!";
                log.Info($"Created Quiz by : {UserName}. Course Record : {Quiz.TrainingQuizId}");
                return RedirectToAction(nameof(Index), new { courseId = courseId });
            }
            catch (Exception ex)
            {
                log.Error($"Error : {ex}");
                return RedirectToAction(nameof(Index), new { courseId = courseId });
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            var courseId = HttpContext.Session.GetString("courseId");
            // Simulate fetching from database                        
            TrainingQuiz quiz = _quiz.getListId(id);
            ViewBag.TrainingQuiz_ModuleId = new SelectList(_quiz.getModuleList(Convert.ToInt32(courseId)).Result.ToList(), "id", "name", quiz.TrainingQuizModuleId);
            //ViewBag.TrainingCourse_TrainingId = new SelectList(TrainngList.Result.ToList(), "TrainingId", "TrainingEname", Course.TrainingCourseTrainingId);
            return PartialView("_EditPartial", quiz);
        }

        [HttpPost]
        public ActionResult Edit(IFormCollection collection)
        {
            var courseId = HttpContext.Session.GetString("courseId");

            try
            {
                var UserName = HttpContext.Session.GetString("UserName");
                var quiz = _quiz.updateQuiz(collection);
                TempData["ToastMessage"] = "UpdatedQuizSuccessfully!";

                log.Info($"Edited Quiz by : {UserName}. Quiz Record : {quiz.TrainingQuizId}");
                return RedirectToAction(nameof(Index), new { courseId = courseId });
            }
            catch (Exception ex)
            {
                log.Error($"Error : {ex}");
                return RedirectToAction(nameof(Index), new { courseId = courseId });
            }
        }

        public async Task<IActionResult> QuizDetails(int id)
        {
            TrainingQuiz quiz = _quiz.getListId(id);
            return PartialView("_DetailPartial", quiz);
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            try
            {
                var UserName = HttpContext.Session.GetString("UserName");
                var quiz = _quiz.DeleteQuiz(id);
                TempData["ToastMessage"] = "DeletedQuizSuccessfully!";

                log.Info($"Deleted Quiz by : {UserName}. Quiz Record : {quiz.TrainingQuizId}");
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                log.Error($"Error : {ex}");
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
