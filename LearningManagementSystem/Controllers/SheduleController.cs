using ComplaignManagementSystem.Presentation.Filters;
using LearningManagementSystem.Bussiness.CourseHandler;
using LearningManagementSystem.Bussiness.SheduleHandler;
using LearningManagementSystem.Data.LMSModels;
using log4net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LearningManagementSystem.Presentation.Controllers
{
    [SessionCheck]
    public class SheduleController : Controller
    {
        private readonly ISheduleService _shedule;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private static readonly ILog log = LogManager.GetLogger(typeof(TrainingController));

        public SheduleController(ISheduleService sheduleService, IWebHostEnvironment webHostEnvironment)
        {
            _shedule = sheduleService;
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
            //var SheduleList = _shedule.getAllList(courseId);
            ViewBag.SheduleList = _shedule.getAllList(courseId);
            var trainingId = HttpContext.Session.GetString("trainingId");
            //var CourseList = _shedule.getCourseList(Convert.ToInt32(trainingId));
            //var TrainerList = _shedule.getTrainerlList();
            ViewBag.TrainingShedule_TrainingCourseId = new SelectList(_shedule.getCourseList(Convert.ToInt32(trainingId)).Result.ToList(), "id", "name");
            ViewBag.TrainingShedule_TrainerId = new SelectList(_shedule.getTrainerlList().Result.ToList(), "id", "name");

            HttpContext.Session.SetString("courseId", courseId.ToString());
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetReference(int trainingCourseId)
        {
            var sheduleId = HttpContext.Session.GetString("sheduleId");
            var allList = _shedule.getAllList(trainingCourseId);
            int recCount;
            if (sheduleId == null)
                recCount = allList.Count;
            else
                recCount = allList.Where(a => a.TrainingSheduleId != Convert.ToInt32(sheduleId)).Count();

            var courseModel = _shedule.getCourseDetail(trainingCourseId);
            var reff = courseModel.TrainingCourseTraining.TrainingEname + "/" + courseModel.TrainingCourseEname + "/" + (recCount + 1).ToString("D3");
            //var CourseList = await _learning.getCourseList(trainingId);
            return Json(reff);
        }

        // POST: DepartmentController/Create
        [HttpPost]
        public ActionResult Create(IFormCollection collection)
        {
            var courseId = HttpContext.Session.GetString("courseId");
            try
            {
                var UserName = HttpContext.Session.GetString("UserName");
                var Shedule = _shedule.CreateShedule(collection);
                TempData["ToastMessage"] = "SubmittedSheduleSuccessfully!";

                log.Info($"Shedule Created by : {UserName}. Shedule Record : {Shedule.TrainingSheduleId}");
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
            HttpContext.Session.SetString("sheduleId", id.ToString());
            // Simulate fetching from database                        
            TrainingShedule shedule = _shedule.getListId(id);
            var trainingId = HttpContext.Session.GetString("trainingId");

            ViewBag.TrainingShedule_TrainerId = new SelectList(_shedule.getTrainerlList().Result.ToList(), "id", "name", shedule.TrainingSheduleTrainerId);
            ViewBag.TrainingShedule_TrainingCourseId = new SelectList(_shedule.getCourseList(Convert.ToInt32(trainingId)).Result.ToList(), "id", "name", shedule.TrainingSheduleTrainingCourseId);
            return PartialView("_EditPartial", shedule);
        }

        [HttpPost]
        public ActionResult Edit(IFormCollection collection)
        {
            var courseId = HttpContext.Session.GetString("courseId");

            try
            {
                var UserName = HttpContext.Session.GetString("UserName");
                var shedule = _shedule.updateShedule(collection);
                TempData["ToastMessage"] = "UpdatedSheduleSuccessfully!";

                log.Info($"Edited Shedule by : {UserName}. Shedule Record : {shedule.TrainingSheduleId}");
                return RedirectToAction(nameof(Index), new { courseId = courseId });
            }
            catch (Exception ex)
            {
                log.Error($"Error : {ex}");
                return RedirectToAction(nameof(Index), new { courseId = courseId });
            }
        }

        public async Task<IActionResult> SheduleDetails(int id)
        {
            TrainingShedule shedule = _shedule.getListId(id);
            return PartialView("_DetailPartial", shedule);
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            try
            {
                var UserName = HttpContext.Session.GetString("UserName");
                var shedule = _shedule.DeleteShedule(id);
                TempData["ToastMessage"] = "DeletedSheduleSuccessfully!";

                log.Info($"Deleted Shedule by : {UserName}. Shedule Record : {shedule.TrainingSheduleId}");
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
