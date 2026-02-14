using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagementSystem.Controllers
{
    public class ReceptionistController : Controller
    {
        // GET: ReceptionistController
        public IActionResult Index()
        {
            // 🔐 Get EmployeeId from session
            int? employeeId = HttpContext.Session.GetInt32("EmployeeId");

            // Optional safety check
            if (employeeId == null || employeeId == 0)
            {
                return RedirectToAction("Index", "Login");
            }

            // Pass it to view
            ViewBag.EmployeeId = employeeId;

            return View();
        }

        // GET: ReceptionistController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ReceptionistController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ReceptionistController/Create
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

        // GET: ReceptionistController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ReceptionistController/Edit/5
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

        // GET: ReceptionistController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ReceptionistController/Delete/5
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
