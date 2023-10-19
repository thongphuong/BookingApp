using Frontend.CustomAuthorize;
using Microsoft.AspNetCore.Mvc;

namespace Frontend.Controllers
{
    [CustomAuthorize]
    public class ProviderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Detail() {
            return View();
        }
        public IActionResult Edit()
        {
            return View();
        }
        public IActionResult Add()
        {
            return View();
        }
    }
}
