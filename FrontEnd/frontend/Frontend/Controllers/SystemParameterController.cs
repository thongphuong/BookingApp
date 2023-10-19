using Frontend.CustomAuthorize;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Frontend.Controllers
{
    [CustomAuthorize]
    public class SystemParameterController : Controller
    {
        // GET: SystemParameterController
        public ActionResult Index()
        {
            if (!Utils.Permission.CheckPermissions("SystemParameter", ""))
                return RedirectToAction("/Error");
            return View();
        }

        // GET: SystemParameterController/Details/5
        public ActionResult Detail(int id)
        {
            if (!Utils.Permission.CheckPermissions("SystemParameter", "Detail"))
                return RedirectToAction("/Error");
            return View();
        }

        // GET: SystemParameterController/Create
        public ActionResult Add()
        {
            if (!Utils.Permission.CheckPermissions("SystemParameter", "Add"))
                return RedirectToAction("/Error");
            return View();
        }

        // POST: SystemParameterController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: SystemParameterController/Edit/5
        public ActionResult Edit(int id)
        {
            if (!Utils.Permission.CheckPermissions("SystemParameter", "Edit"))
                return RedirectToAction("/Error");
            return View();
        }

        // POST: SystemParameterController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: SystemParameterController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SystemParameterController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
