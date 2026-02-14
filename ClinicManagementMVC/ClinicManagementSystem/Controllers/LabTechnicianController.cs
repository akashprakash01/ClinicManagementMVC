using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagementSystem.Controllers
{
    public class LabTechnicianController : Controller
    {
        // GET: LabTechnicianController
        public ActionResult Index()
        {
            return View();
        }

        // GET: LabTechnicianController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: LabTechnicianController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LabTechnicianController/Create
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

        // GET: LabTechnicianController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: LabTechnicianController/Edit/5
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

        // GET: LabTechnicianController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: LabTechnicianController/Delete/5
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
