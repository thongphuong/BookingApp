using Frontend.CustomAuthorize;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Frontend.Controllers
{
    [CustomAuthorize]
    public class TimeFrameController : Controller
    {
        // GET: TimeFrameController
        public ActionResult Index()
        {
            if (!Utils.Permission.CheckPermissions("TimeFrame", ""))
                return RedirectToAction("/Error");
            return View();
        }

        // GET: TimeFrameController/Details/5
        public ActionResult Detail(int id)
        {
            if (!Utils.Permission.CheckPermissions("TimeFrame", "Detail"))
                return RedirectToAction("/Error");
            return View();
        }

        // GET: TimeFrameController/Create
        public ActionResult Add()
        {
            if (!Utils.Permission.CheckPermissions("TimeFrame", "Add"))
                return RedirectToAction("/Error");
            return View();
        }

        public ActionResult AddQuick()
        {
            return View();
        }

        // POST: TimeFrameController/Create
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

        // GET: TimeFrameController/Edit/5
        public ActionResult Edit(int id)
        {
            if (!Utils.Permission.CheckPermissions("TimeFrame", "Edit"))
                return RedirectToAction("/Error");
            return View();
        }

        // POST: TimeFrameController/Edit/5
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

        // GET: TimeFrameController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TimeFrameController/Delete/5
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
