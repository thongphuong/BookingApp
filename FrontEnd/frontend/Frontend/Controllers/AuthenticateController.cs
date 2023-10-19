using Frontend.Models;
using Frontend.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Frontend.Controllers
{
    public class AuthenticateController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        [Route("ApplyLogin")]
        [HttpPost]
        public bool ApplyLogin(AuthResultModel AutResult)
        {
            try
            {
                if (AutResult == null)
                {
                    return false;
                }
                var generatedToken = AutResult.Token;
                if (string.IsNullOrEmpty(generatedToken))
                {
                    return false;
                }
                HttpContext.Session.SetString("SessionToken", generatedToken);
                HttpContext.Session.Set<CurrentUserModel>("UserInfo", AutResult.currentUsersModel);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
                Response.Cookies.Delete("language");
                //var _language = "vi-VN";
                var _language = "en";
                var option = new CookieOptions()
                {
                    Expires = DateTime.Now.AddDays(1)
                };
                Response.Cookies.Append("language", _language, option);
                Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName, CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(_language)), option);
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }


        [AllowAnonymous]
        [HttpPost]
        public string MenuComponent([FromBody] List<MenuModel> apiResponse)
        {
            var result = "";
            try
            {
                var json = JsonSerializer.Serialize(apiResponse);
                result = json;
                HttpContext.Session.SetString("MenuData", json);
            }
            catch (Exception)
            {
            }
            return result;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Redirect("/Authenticate");
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult ChangeLanguage(string key_value)
        {
            var _language = "en";
            if (key_value != null)
            {
                _language = key_value;
            }
            Response.Cookies.Delete("language");
            CookieOptions option = new()
            {
                Expires = DateTime.Now.AddDays(1)
            };
            Response.Cookies.Append("language", _language, option);
            Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName, CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(_language)), option);
            return new JsonResult(new { result = true });
        }
    }
}
