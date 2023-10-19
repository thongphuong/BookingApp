using Microsoft.AspNetCore.Mvc;
using Frontend.CustomAuthorize;

namespace Frontend.Controllers
{
    [CustomAuthorize]
    public class LinkController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Detail()
        {
            return View();
        }
        public IActionResult Add()
        {
            return View();
        }
        public IActionResult Edit()
        {
            return View();
        }
    }
}
