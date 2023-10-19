using Frontend.CustomAuthorize;
using Microsoft.AspNetCore.Mvc;

namespace Frontend.Controllers
{
    [CustomAuthorize]
    public class OrderController : Controller
    {
        public IActionResult Add()
        {
            if (!Utils.Permission.CheckPermissions("User", "Detail"))
                return RedirectToAction("/Error");
            return View();
        }

        public IActionResult Index()
        {
            if (!Utils.Permission.CheckPermissions("User",""))
                return RedirectToAction("/Error");
            return View();
        }

        public IActionResult Detail()
        {
            if (!Utils.Permission.CheckPermissions("Order", "Detail", "Booking"))
                return RedirectToAction("/Error");
            return View();
        }

        public IActionResult Edit()
        {
            if (!Utils.Permission.CheckPermissions("Order", "Edit"))
                return RedirectToAction("/Error");
            return View();
        }
        public IActionResult Approve()
        {
            if (!Utils.Permission.CheckPermissions("Order", "Approve"))
                return RedirectToAction("/Error");
            return View();
        }

        public IActionResult Return()
        {
            if (!Utils.Permission.CheckPermissions("Order", "Return"))
                return RedirectToAction("/Error");
            return View();
        }
        public IActionResult Refuse()
        {
            if (!Utils.Permission.CheckPermissions("Order", "Refuse"))
                return RedirectToAction("/Error");
            return View();
        }
    }
}
