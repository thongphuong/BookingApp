using Frontend.CustomAuthorize;
using Microsoft.AspNetCore.Mvc;

namespace Frontend.Controllers
{
    [CustomAuthorize]
    public class ProviderOrderController : Controller
    {
        public IActionResult LineCode()
        {
            return View();
        }
        public IActionResult OrderInfo()
        {
            return View();
        }
        public IActionResult OrderDetail()
        {
            return View();
        }
        public IActionResult OrderDeniedInfo()
        {
            return View();
        }
        public IActionResult OrderDeniedInfoPrint()
        {
            return View();
        }
        public IActionResult Approve()
        {
            return View();
        }
        public IActionResult Denied()
        {
            return View();
        }
        public IActionResult OrderDenied()
        {
            return View();
        }
    }
}
