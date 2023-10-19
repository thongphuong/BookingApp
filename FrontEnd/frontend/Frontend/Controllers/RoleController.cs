using Frontend.CustomAuthorize;
using Microsoft.AspNetCore.Mvc;

namespace Frontend.Controllers
{
    [CustomAuthorize]
    public class RoleController : Controller
    {
        public IActionResult Index()
        {
            if (!Utils.Permission.CheckPermissions("Role", ""))
                return RedirectToAction("/Error");
            return View();
        }
        public IActionResult Add()
        {
            if (!Utils.Permission.CheckPermissions("Role", "Add"))
                return RedirectToAction("/Error");
            return View();
        }
        public IActionResult Detail()
        {
            if (!Utils.Permission.CheckPermissions("Role", "Detail"))
                return RedirectToAction("/Error");
            return View();
        }
        public IActionResult Edit()
        {
            if (!Utils.Permission.CheckPermissions("Role", "Edit"))
                return RedirectToAction("/Error");
            return View();
        }
    }
}
