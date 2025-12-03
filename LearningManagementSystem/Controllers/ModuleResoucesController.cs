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
        private readonly string _attachmentsFolder = @"E:\Attachments\";
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
            HttpContext.Session.SetString("moduleResourceId", id.ToString());
            // Simulate fetching from database                        
            TrainingCourseModuleResource resource = _moduleResourse.getListId(id);

            ViewBag.TrainingCourseModuleResources_ModuleId = new SelectList(_moduleResourse.getModulelList(Convert.ToInt32(courseId)).Result.ToList(), "id", "name", resource.TrainingCourseModuleResourcesModuleId);

            return PartialView("_EditPartial", resource);
        }

        [HttpGet]
        public JsonResult CheckSequence(int moduleId, int sequence)
        {
            // Example - check if sequence exists in DB
            var moduleResId = HttpContext.Session.GetString("moduleResourceId");

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

        [HttpGet]
        public IActionResult GetVideoFile(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return BadRequest();

            var filePath = Path.Combine(_attachmentsFolder, fileName);
            if (!System.IO.File.Exists(filePath))
                return NotFound();

            var stream = System.IO.File.OpenRead(filePath);
            return File(stream, "video/mp4", enableRangeProcessing: true);
        }

        [HttpPost]
        public ActionResult Edit(IFormCollection collection, IFormFile file)
        {
            var courseId = HttpContext.Session.GetString("courseId");
            try
            {
                var UserName = HttpContext.Session.GetString("UserName");
                var ModuleResource = _moduleResourse.UpdateResource(collection, file);
                TempData["ToastMessage"] = "UpdatedModuleResourceSuccessfully!";
                log.Info($"Edited Resource by : {UserName}. Resource Record : {ModuleResource.TrainingCourseModuleResourcesId}");
                return RedirectToAction(nameof(Index), new { courseId = courseId });
            }
            catch (Exception ex)
            {
                log.Error($"Error : {ex}");
                return RedirectToAction(nameof(Index), new { courseId = courseId });
            }
        }

        [HttpPost]
        public JsonResult DeleteAttachment(int id)
        {
            try
            {
                //string filePath = Path.Combine($"wwwroot/Attachments/_{id}");
                var UserName = HttpContext.Session.GetString("UserName");
                string fileName = $@"_{id}.mp4";

                // Combine with your attachments folder
                string fullPath = Path.Combine(_attachmentsFolder, fileName);

                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                    var ModuleResource = _moduleResourse.DeleteAttachment(id);
                    log.Info($"Successfully deleted attachment: {fileName} by user: {UserName}");
                }
                else
                {
                    log.Warn($"Attachment not found: {fileName}. Attempted by user: {UserName}");
                }


                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                log.Error($"Error Complain Saving : {ex.Message}. Record : {id}.");
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult DeletePdfAttachment(int id)
        {
            try
            {
                //string filePath = Path.Combine($"wwwroot/Attachments/_{id}");
                var UserName = HttpContext.Session.GetString("UserName");
                string fileName = $@"_{id}.pdf";

                // Combine with your attachments folder
                string fullPath = Path.Combine(_attachmentsFolder, fileName);

                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                    var ModuleResource = _moduleResourse.DeleteAttachment(id);
                    log.Info($"Successfully deleted attachment: {fileName} by user: {UserName}");
                }
                else
                {
                    log.Warn($"Attachment not found: {fileName}. Attempted by user: {UserName}");
                }


                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                log.Error($"Error Complain Saving : {ex.Message}. Record : {id}.");
                return Json(new { success = false, message = ex.Message });
            }
        }

        public IActionResult DownloadAttachment(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return NotFound("Filename is not specified.");
            }

            var path = Path.Combine(_attachmentsFolder, fileName);
            if (!System.IO.File.Exists(path))
            {
                return NotFound("File not found.");
            }

            var mimeType = "application/octet-stream"; // A generic MIME type for file downloads
            var fileBytes = System.IO.File.ReadAllBytes(path);

            return File(fileBytes, mimeType, fileName);
        }

        public async Task<IActionResult> ModuleResourceDetails(int id)
        {
            TrainingCourseModuleResource resource = _moduleResourse.getListId(id);
            return PartialView("_DetailPartial", resource);
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            try
            {
                var UserName = HttpContext.Session.GetString("UserName");
                var module = _moduleResourse.DeleteResource(id);
                TempData["ToastMessage"] = "DeletedModuleResourceSuccessfully!";

                log.Info($"Deleted Rescource by : {UserName}. Rescource Record : {id}");
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
