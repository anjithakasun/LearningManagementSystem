using ComplaignManagementSystem.Presentation.Filters;
using LearningManagementSystem.Bussiness.CourseHandler;
using LearningManagementSystem.Bussiness.ResourcesHandler;
using LearningManagementSystem.Data.LMSModels;
using log4net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Reflection;

namespace LearningManagementSystem.Presentation.Controllers
{
    [SessionCheck]
    public class ModuleResoucesController : Controller
    {
        private readonly IModuleResourceService _moduleResourse;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private static readonly ILog log = LogManager.GetLogger(typeof(TrainingController));

        public ModuleResoucesController(IModuleResourceService moduleResource, IWebHostEnvironment webHostEnvironment)
        {
            _moduleResourse = moduleResource;
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
            var ModuleList = _moduleResourse.getModulelList(courseId);
            ViewBag.ResourceList = _moduleResourse.getAllList(courseId);
            //var TrainngList = _course.getTrainingList();
            ViewBag.TrainingCourseModuleResources_ModuleId = new SelectList(ModuleList.Result.ToList(), "id", "name");
            ViewBag.CourseName = _moduleResourse.GetCourseName(courseId);
            return View();
        }

        [HttpPost]
        public ActionResult Create(IFormCollection collection, IFormFile file)
        {
            var courseId = HttpContext.Session.GetString("courseId");
            try
            {
                var UserName = HttpContext.Session.GetString("UserName");
                var ModuleResource = _moduleResourse.CreateResource(collection, file);
                TempData["ToastMessage"] = "SubmittedModuleResourceSuccessfully!";
                log.Info($"Created Resource by : {UserName}. Resource Record : {ModuleResource.TrainingCourseModuleResourcesId}");
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
            HttpContext.Session.SetString("moduleResId", id.ToString());
            // Simulate fetching from database                        
            TrainingCourseModuleResource resource = _moduleResourse.getListId(id);

            ViewBag.TrainingCourseModule_TrainingCourseId = new SelectList(_moduleResourse.getModulelList(Convert.ToInt32(courseId)).Result.ToList(), "id", "name", resource.TrainingCourseModuleResourcesModuleId);

            return PartialView("_EditPartial", resource);
        }

        [HttpGet]
        public JsonResult CheckSequence(int moduleId, int sequence)
        {
            // Example - check if sequence exists in DB
            var moduleResId = HttpContext.Session.GetString("moduleResId");

            var moduleResList = _moduleResourse.getCheckList(moduleId);
            bool exists;
            if (moduleResId == null)
                exists = moduleResList.Any(a => a.TrainingCourseModuleResourcesSequance == sequence);
            else
                exists = moduleResList.Where(a => a.TrainingCourseModuleResourcesId != Convert.ToInt32(moduleResId)).Any(a => a.TrainingCourseModuleResourcesSequance == sequence);

            if (exists)
            {
                return Json(new { status = false });
            }
            else
            {
                return Json(new { status = true });
            }
        }
    }
}
