using Frontend.CustomAuthorize;
﻿using Frontend.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Frontend.Controllers
{
    [CustomAuthorize]
    public class StoreController : Controller
    {
        private IHttpClientFactory factory;
        public StoreController(IHttpClientFactory _factory) {
            factory = _factory;
        }
        public IActionResult Index()
        {
            if (!Utils.Permission.CheckPermissions("Store", ""))
                return RedirectToAction("/Error");
            return View();
        }
        public IActionResult Add()
        {
            if (!Utils.Permission.CheckPermissions("Store", "Add"))
                return RedirectToAction("/Error");
            return View();
        }

        [HttpGet]        
        public IActionResult Edit()
        {
            if (!Utils.Permission.CheckPermissions("Store", "Edit"))
                return RedirectToAction("/Error");
            return View();
        }

        [HttpGet] 
        public IActionResult Detail()
        {
            if (!Utils.Permission.CheckPermissions("Store", "Detail"))
                return RedirectToAction("/Error");
            return View();
        }

    }
}
