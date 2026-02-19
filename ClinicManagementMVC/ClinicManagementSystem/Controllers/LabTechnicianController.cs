using ClinicManagementSystem.Models;
using ClinicManagementSystem.Service;
using ClinicManagementSystem.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.Filters;


namespace ClinicManagementSystem.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class LabTechnicianController : Controller
    {
        private readonly ILabTechnicianService _service;

        public LabTechnicianController(ILabTechnicianService service)
        {
            _service = service;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            int? roleId = HttpContext.Session.GetInt32("RoleId");

            if (roleId == null || roleId != 4)
            {
                context.Result = RedirectToAction("Index", "Login");
                return;
            }

            base.OnActionExecuting(context);
        }
        public IActionResult Index()
        {
            var pending = _service.GetPendingLabTests();
            var completed = _service.GetCompletedLabTests();

            ViewBag.CompletedTests = completed;

            return View(pending);
        }

        // GET
        public IActionResult AddLabTest()
        {
            return View();
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddLabTest(LabTestVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            string message;
            int id = _service.AddLabTest(model, out message);

            TempData["Message"] = message;

            return RedirectToAction("AddLabTest");
        }

        public IActionResult AddResult(int id)
        {
            // id = PrescriptionLabTestId

            var model = _service.GetLabTestDetailsForResult(id);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddResult(LabTestResultVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            _service.AddLabTestResult(model);

            return RedirectToAction("Index");
        }
        public IActionResult EditResult(int id)
        {
            var model = _service.GetLabTestResultById(id);
            return View(model);
        }
        [HttpPost]
        public IActionResult EditResult(LabTestResultVM model)
        {
            _service.UpdateLabTestResult(model);
            return RedirectToAction("Index");
        }

        public IActionResult ViewBill(int id)
        {
            try
            {
                var model = _service.GetPrescriptionLabBill(id);

                // If no tests returned, show message
                if (model == null || model.Tests == null || model.Tests.Count == 0)
                {
                    TempData["Error"] = "All lab tests are not completed yet.";
                    return RedirectToAction("Index");
                }

                return View(model);
            }
            catch (SqlException ex)
            {
                // Friendly message instead of crash
                if (ex.Message.Contains("All lab tests are not completed"))
                {
                    TempData["Error"] = "All lab tests must be completed before generating the bill.";
                    return RedirectToAction("Index");
                }

                throw;
            }


        }
        public IActionResult PrintLabBill(int id)
        {
            var model = _service.GetPrescriptionLabBill(id);

            if (model == null)
                return NotFound();

            return View("PrintLabBill", model);
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Login");
        }

    }
}