using Frontend.CustomAuthorize;
using Microsoft.AspNetCore.Mvc;

namespace Frontend.Controllers
{
    [CustomAuthorize]
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            if (!Utils.Permission.CheckPermissions("User", ""))
                return RedirectToAction("/Error");
            return View();
        }
        public IActionResult Detail()
        {
            if (!Utils.Permission.CheckPermissions("User", "Detail"))
                return RedirectToAction("/Error");
            return View();
        }
        public IActionResult Add()
        {
            if (!Utils.Permission.CheckPermissions("User", "Add"))
                return RedirectToAction("/Error");
            return View();
        }
        public IActionResult Edit()
        {
            if (!Utils.Permission.CheckPermissions("User", "Edit"))
                return RedirectToAction("/Error");
            return View();
        }
        public IActionResult UserInfo()
        {
            return View();
        }
        public IActionResult ChangePass()
        {
            return View();
        }

        public IActionResult Profile()
        {
            if (!Utils.Permission.CheckPermissions("User", "Detail"))
                return RedirectToAction("/Error");
            return View();
        }
    }
}
