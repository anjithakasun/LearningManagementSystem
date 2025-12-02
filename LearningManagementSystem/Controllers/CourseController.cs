using ComplaignManagementSystem.Presentation.Filters;
using LearningManagementSystem.Bussiness.CourseHandler;
using LearningManagementSystem.Bussiness.TrainingHandler;
using LearningManagementSystem.Data.LMSModels;
using log4net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace LearningManagementSystem.Presentation.Controllers
{
    [SessionCheck]
    public class CourseController : Controller
    {

        private readonly ICourseService _course;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private static readonly ILog log = LogManager.GetLogger(typeof(TrainingController));

        public CourseController(ICourseService courseService, IWebHostEnvironment webHostEnvironment)
        {
            _course = courseService;
            _webHostEnvironment = webHostEnvironment;
        }

        private bool IsUserLoggedIn()
        {
            return !string.IsNullOrEmpty(HttpContext.Session.GetString("UserName"));
        }
        [HttpGet]
        public IActionResult Index(int trainingId)
        {
            if (!IsUserLoggedIn())
                return RedirectToAction("Login", "User");
            HttpContext.Session.SetString("trainingId", trainingId.ToString());
            ViewBag.TrainingName = _course.GetTrainingName(trainingId);
            ViewBag.CourseList = _course.getAllList(trainingId);
            ViewBag.TrainingCourse_TrainingId = new SelectList(_course.getTrainingList().Result.ToList(), "TrainingId", "TrainingEname");

            HttpContext.Session.SetString("trainingId", trainingId.ToString());
            return View();
        }

        // POST: DepartmentController/Create
        [HttpPost]
        public ActionResult Create(IFormCollection collection)
        {
            var trainingId = HttpContext.Session.GetString("trainingId");
            try
            {
                var UserName = HttpContext.Session.GetString("UserName");                
                var Course = _course.CreateCourse(collection);
                TempData["ToastMessage"] = "SubmittedCourseSuccessfully!";
                log.Info($"Created Course by : {UserName}. Course Record : {Course.TrainingCourseId}");
                return RedirectToAction(nameof(Index), new { trainingId = trainingId });
            }
            catch (Exception ex)
            {
                log.Error($"Error : {ex}");
                return RedirectToAction(nameof(Index), new { trainingId = trainingId });
            }
        }

        [HttpGet]
        public JsonResult CheckSequence(int trainingId, int sequence)
        {
            // Example - check if sequence exists in DB
            var courseId = HttpContext.Session.GetString("courseId");
            var CourseList = _course.getAllList(trainingId);
            bool exists;
            if (courseId == null)
                exists = CourseList.Any(a => a.TrainingCourseSequance == sequence);
            else
                exists = CourseList.Where(a => a.TrainingCourseId != Convert.ToInt32(courseId)).Any(a => a.TrainingCourseSequance == sequence);

            if (exists)
            {
                return Json(new { status = false });
            }
            else
            {
                return Json(new { status = true });
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            HttpContext.Session.SetString("courseId", id.ToString());
            // Simulate fetching from database                        
            TrainingCourse Course = _course.getListId(id);
            var TrainngList = _course.getTrainingList();
            ViewBag.TrainingCourse_TrainingId = new SelectList(TrainngList.Result.ToList(), "TrainingId", "TrainingEname", Course.TrainingCourseTrainingId);
            return PartialView("_EditPartial", Course);
        }

        [HttpPost]
        public ActionResult Edit(IFormCollection collection)
        {
            var trainingId = HttpContext.Session.GetString("trainingId");

            try
            {
                var UserName = HttpContext.Session.GetString("UserName");
                var Course = _course.updateCourse(collection);
                TempData["ToastMessage"] = "UpdatedCourseSuccessfully!";

                log.Info($"Edited Course by : {UserName}. Course Record : {Course.TrainingCourseId}");
                return RedirectToAction(nameof(Index), new { trainingId = trainingId });
            }
            catch (Exception ex)
            {
                log.Error($"Error : {ex}");
                return RedirectToAction(nameof(Index), new { trainingId = trainingId });
            }
        }

        public async Task<IActionResult> TairningDetails(int id)
        {
            TrainingCourse Course = _course.getListId(id);
            return PartialView("_DetailPartial", Course);
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            try
            {                
                var UserName = HttpContext.Session.GetString("UserName");
                var course = _course.DeleteCourse(id);
                TempData["ToastMessage"] = "DeletedCourseSuccessfully!";

                log.Info($"Deleted Course by : {UserName}. Course Record : {course.TrainingCourseId}");
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
