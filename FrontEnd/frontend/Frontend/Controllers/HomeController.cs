using Frontend.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Frontend.CustomAuthorize;
using Microsoft.AspNetCore.Localization;

namespace Frontend.Controllers
{
    [CustomAuthorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CultureManagement(string culture, string returnUrl)
        {
            Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName, CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)), new CookieOptions { Expires = DateTimeOffset.Now.AddDays(1) });
            return LocalRedirect(returnUrl);
        }
    }
}