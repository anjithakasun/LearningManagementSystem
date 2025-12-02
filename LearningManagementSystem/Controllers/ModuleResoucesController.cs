using ComplaignManagementSystem.Presentation.Filters;
using LearningManagementSystem.Bussiness.CourseHandler;
using log4net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LearningManagementSystem.Presentation.Controllers
{
    [SessionCheck]
    public class ModuleResoucesController : Controller
    {
        private readonly ICourseService _course;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private static readonly ILog log = LogManager.GetLogger(typeof(TrainingController));

        public ModuleResoucesController(ICourseService courseService, IWebHostEnvironment webHostEnvironment)
        {
            _course = courseService;
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
            //HttpContext.Session.SetString("trainingId", trainingId.ToString());
            //var CourseList = _course.getAllList(trainingId);
            //ViewBag.CourseList = _course.getAllList(trainingId);
            //var TrainngList = _course.getTrainingList();
            //ViewBag.TrainingCourse_TrainingId = new SelectList(TrainngList.Result.ToList(), "TrainingId", "TrainingEname");

            //HttpContext.Session.SetString("trainingId", trainingId.ToString());
            return View();
        }
    }
}
