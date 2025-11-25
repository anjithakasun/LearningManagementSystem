using ComplaignManagementSystem.Presentation.Filters;
using LearningManagementSystem.Bussiness.UserHandler;
using LearningManagementSystem.Data.LMSModels;
using log4net;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.Presentation.Controllers
{
    [SessionCheck]
    public class UserController : Controller
    {
        private readonly IUserService _loginService;
        private static readonly ILog log = LogManager.GetLogger(typeof(UserController));

        public UserController(IUserService loginService)
        {
            _loginService = loginService;
        }

        public IActionResult Index()
        {
            return View();
        }

        private bool IsUserLoggedIn()
        {
            return !string.IsNullOrEmpty(HttpContext.Session.GetString("UserName"));
        }

        // GET: UserController
        public ActionResult Login()
        {
            Response.Cookies.Delete(".AspNetCore.Session");
            HttpContext.Session.Clear();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            try
            {
                User user = await _loginService.ValidateUserAsync(username, password);

                if (user != null)
                {
                    //var DepDetails = _loginService.GetDepartmentDetails(Convert.ToInt32(user.Dep_Id));

                    HttpContext.Session.SetString("UserName", user.UserName);
                    //HttpContext.Session.SetString("UserDepName", DepDetails.Name);
                    //HttpContext.Session.SetString("UserDep_Id", Convert.ToString(user.Dep_Id));
                    ////HttpContext.Session.SetString("SaltKey", Convert.ToString(user.SaltKey));
                    //HttpContext.Session.SetString("UserId", Convert.ToString(user.Id));

                    //if (user.IsReset == false)
                    //{
                    //    //return RedirectToAction("Reset");
                    //    return Json(new { success = false, redirectUrl = Url.Action("Reset", "User") });
                    //}

                    //UserPermissionModel getPermissions = _loginService.getAccessPerimissions(user);
                    //var getAccessPages = _loginService.getAccessPages(user, getPermissions);

                    //var DepUCount = getAccessPages.Where(a => a.Page == "Department Master" && a.Active == true).Count();
                    //var CentUCount = getAccessPages.Where(a => a.Page == "Central Master" && a.Active == true).Count();

                    //HttpContext.Session.SetString("UserPermission", getPermissions.Role);
                    //if (getPermissions.Role != "User" && DepUCount != 0)
                    //{
                    //    HttpContext.Session.SetString("DepartmentPermission", "Department User");
                    //}
                    //else
                    //{
                    //    HttpContext.Session.SetString("DepartmentPermission", "");
                    //}
                    //if (getPermissions.Role != "User" && CentUCount != 0)
                    //{
                    //    HttpContext.Session.SetString("CentralPermission", "Central User");
                    //}
                    //else
                    //{
                    //    HttpContext.Session.SetString("CentralPermission", "");
                    //}
                    //var jsonData = JsonConvert.SerializeObject(getAccessPages);

                    //HttpContext.Session.SetString("AccessPages", jsonData);
                    log.Info($"Success Login by : {user.UserName}.");
                    return Json(new { success = true, redirectUrl = Url.Action("Dashboard", "LearningManagement") });
                }
                else
                {
                    log.Info($"Failed login.");
                    return Json(new { success = false });
                }
            }
            catch (Exception ex)
            {
                log.Error($"Error Login : {ex.Message}.");
                throw;
            }

        }
    }
}
