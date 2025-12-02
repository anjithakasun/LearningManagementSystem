using ComplaignManagementSystem.Presentation.Filters;
using LearningManagementSystem.Bussiness.CourseHandler;
using LearningManagementSystem.Bussiness.ModuleHandler;
using LearningManagementSystem.Data.LMSModels;
using log4net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LearningManagementSystem.Presentation.Controllers
{
    [SessionCheck]
    public class ModuleController : Controller
    {
        private readonly IModuleService _module;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private static readonly ILog log = LogManager.GetLogger(typeof(TrainingController));

        public ModuleController(IModuleService moduleService, IWebHostEnvironment webHostEnvironment)
        {
            _module = moduleService;
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
            //TempData["ToastMessage"] = "SubmittedModuleSuccessfully!";
            ViewBag.ModuleList = _module.getAllList(trainingId);
            var CourseList = _module.getCourseList(Convert.ToInt32(trainingId));
            ViewBag.TrainingName = _module.GetTrainingName(trainingId);

            ViewBag.TrainingCourseModule_TrainingCourseId = new SelectList(_module.getCourseList(Convert.ToInt32(trainingId)).Result.ToList(), "id", "name");
            ViewBag.TrainingCourseModule_QuizTypeId = new SelectList(_module.getAuizTypeList().Result.ToList(), "id", "name");
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
                var Module = _module.CreateModule(collection);
                TempData["ToastMessage"] = "SubmittedModuleSuccessfully!";
                log.Info($"Created Module by : {UserName}. Module Record : {Module.TrainingCourseModuleId}");
                return RedirectToAction(nameof(Index), new { trainingId = trainingId });
            }
            catch (Exception ex)
            {
                log.Error($"Error : {ex}");
                return RedirectToAction(nameof(Index), new { trainingId = trainingId });
            }
        }
        

        public async Task<IActionResult> Edit(int id)
        {
            var trainingId = HttpContext.Session.GetString("trainingId");
            HttpContext.Session.SetString("moduleId", id.ToString());
            // Simulate fetching from database                        
            TrainingCourseModule module = _module.getListId(id);

            ViewBag.TrainingCourseModule_TrainingCourseId = new SelectList(_module.getCourseList(Convert.ToInt32(trainingId)).Result.ToList(), "id", "name", module.TrainingCourseModuleTrainingCourseId);
            ViewBag.TrainingCourseModule_QuizTypeId = new SelectList(_module.getAuizTypeList().Result.ToList(), "id", "name", module.TrainingCourseModuleQuizTypeId);

            return PartialView("_EditPartial", module);
        }


        [HttpGet]
        public JsonResult CheckSequence(int courseId, int sequence)
        {
            // Example - check if sequence exists in DB
            var moduleId = HttpContext.Session.GetString("moduleId");

            var moduleList = _module.getCheckList(courseId);
            bool exists;
            if (moduleId == null)
                exists = moduleList.Any(a => a.TrainingCourseModuleSequance == sequence);
            else
                exists = moduleList.Where(a => a.TrainingCourseModuleId != Convert.ToInt32(moduleId)).Any(a => a.TrainingCourseModuleSequance == sequence);

            if (exists)
            {
                return Json(new { status = false });
            }
            else
            {
                return Json(new { status = true });
            }
        }
        [HttpPost]
        public ActionResult Edit(IFormCollection collection)
        {
            var trainingId = HttpContext.Session.GetString("trainingId");

            try
            {
                var UserName = HttpContext.Session.GetString("UserName");
                var module = _module.updateModule(collection);
                TempData["ToastMessage"] = "UpdatedModuleSuccessfully!";

                log.Info($"Edited Module by : {UserName}. Module Record : {module.TrainingCourseModuleId}");
                return RedirectToAction(nameof(Index), new { trainingId = trainingId });
            }
            catch (Exception ex)
            {
                log.Error($"Error : {ex}");
                return RedirectToAction(nameof(Index), new { trainingId = trainingId });
            }
        }

        public async Task<IActionResult> ModuleDetails(int id)
        {
            TrainingCourseModule module = _module.getListId(id);
            return PartialView("_DetailPartial", module);
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            try
            {
                var UserName = HttpContext.Session.GetString("UserName");
                var module = _module.DeleteModule(id);
                TempData["ToastMessage"] = "DeletedModuleSuccessfully!";

                log.Info($"Deleted Module by : {UserName}. Module Record : {module.TrainingCourseModuleId}");
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
