using Microsoft.AspNetCore.Mvc;

namespace Frontend.Controllers
{
    public class RefuseController : Controller
    {
        public IActionResult Index()
        {
            if (!Utils.Permission.CheckPermissions("Order", "", "Refuse"))
                return RedirectToAction("/Error");
            return View();
        }
        public IActionResult Detail()
        {
            if (!Utils.Permission.CheckPermissions("Order", "Detail", "Refuse"))
                return RedirectToAction("/Error");
            return View();
        }

        public IActionResult Print()
        {
            if (!Utils.Permission.CheckPermissions("Order", "Print"))
                return RedirectToAction("/Error");
            return View();
        }
    }
}
