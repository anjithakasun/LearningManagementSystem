using ComplaignManagementSystem.Presentation.Filters;
using LearningManagementSystem.Bussiness.LearningManagementHandler;
using LearningManagementSystem.Bussiness.TrainingHandler;
using log4net;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.Presentation.Controllers
{
    [SessionCheck]
    public class LearningManagementController : Controller
    {
        private readonly ILearningService _learning;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private static readonly ILog log = LogManager.GetLogger(typeof(TrainingController));

        public LearningManagementController(ILearningService learningService, IWebHostEnvironment webHostEnvironment)
        {
            _learning = learningService;
            _webHostEnvironment = webHostEnvironment;
        }

        private bool IsUserLoggedIn()
        {
            return !string.IsNullOrEmpty(HttpContext.Session.GetString("UserName"));
        }

        public ActionResult Dashboard()
        {
            if (!IsUserLoggedIn())
                return RedirectToAction("Login", "User");

            ViewBag.CurrYear = System.DateTime.Now.Year;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetTrainingList()
        {
            var TrainingList = await _learning.getTrainingList();
            return Json(TrainingList);
        }

        [HttpGet]
        public async Task<IActionResult> GetCourseList(int trainingId)
        {
            HttpContext.Session.SetString("trainingId", trainingId.ToString());
            var CourseList = await _learning.getCourseList(trainingId);
            return Json(CourseList);
        }

    }
}
