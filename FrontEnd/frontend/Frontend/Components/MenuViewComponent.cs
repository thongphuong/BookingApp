using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using Frontend.Models;

namespace Frontend.Components
{
    [ViewComponent(Name = "Menu")]
    public class MenuViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            await Task.Delay(10);
            var menuModels = new List<MenuModel>();
            try
            {
                string apiResponse = HttpContext.Session.GetString("MenuData") ?? "";
                if (!string.IsNullOrEmpty(apiResponse))
                {
                    menuModels = JsonSerializer.Deserialize<List<MenuModel>>(apiResponse);
                }

            }
            catch (Exception)
            {
            }

            return View(menuModels);
        }
    }
}
