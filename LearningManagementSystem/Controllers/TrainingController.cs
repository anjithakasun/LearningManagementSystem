using ComplaignManagementSystem.Presentation.Filters;
using LearningManagementSystem.Bussiness.TrainingHandler;
using LearningManagementSystem.Data.LMSModels;
using log4net;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.Presentation.Controllers
{
    [SessionCheck]
    public class TrainingController : Controller
    {
        private readonly ITrainingService _training;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private static readonly ILog log = LogManager.GetLogger(typeof(TrainingController));

        public TrainingController(ITrainingService trainingService, IWebHostEnvironment webHostEnvironment)
        {
            _training = trainingService;
            _webHostEnvironment = webHostEnvironment;
        }

        private bool IsUserLoggedIn()
        {
            return !string.IsNullOrEmpty(HttpContext.Session.GetString("UserName"));
        }

        public IActionResult Index()
        {
            if (!IsUserLoggedIn())
                return RedirectToAction("Login", "User");
            //TempData["ToastMessage"] = "SubmittedTrainingSuccessfully!";
            ViewBag.TrainingList = _training.getAllList();
            return View();
        }

        // POST: DepartmentController/Create
        [HttpPost]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                var UserName = HttpContext.Session.GetString("UserName");
                var training = _training.CreateTraining(collection);
                TempData["ToastMessage"] = "SubmittedTrainingSuccessfully!";
                log.Info($"Created Trainng by : {UserName}. Training Record : {training.TrainingId}");
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            // Simulate fetching from database                        
            Training traning = _training.getListId(id);
            return PartialView("_EditPartial", traning);
        }

        [HttpPost]
        public ActionResult Edit(IFormCollection collection)
        {
            try
            {
                var UserName = HttpContext.Session.GetString("UserName");
                var training = _training.updateTraining(collection);
                TempData["ToastMessage"] = "UpdatedTrainingSuccessfully!";

                log.Info($"Edited Trainng by : {UserName}. Training Record : {training.TrainingId}");
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }

        public async Task<IActionResult> TairningDetails(int id)
        {
            Training traning = _training.getListId(id);
            return PartialView("_DetailPartial", traning);
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            try
            {
                var UserName = HttpContext.Session.GetString("UserName");
                var training = _training.DeleteTraining(id);
                TempData["ToastMessage"] = "DeletedTrainingSuccessfully!";

                log.Info($"Deleted Trainng by : {UserName}. Training Record : {training.TrainingId}");
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
