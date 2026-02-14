using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagementSystem.Controllers
{
    public class PharmacistController : Controller
    {
        // GET: PharmacistController
        public ActionResult Index()
        {
            return View();
        }

        // GET: PharmacistController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PharmacistController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PharmacistController/Create
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

        // GET: PharmacistController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PharmacistController/Edit/5
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

        // GET: PharmacistController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PharmacistController/Delete/5
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
