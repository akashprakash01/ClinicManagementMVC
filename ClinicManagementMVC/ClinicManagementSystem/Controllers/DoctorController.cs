using ClinicManagementSystem.Models;
using ClinicManagementSystem.Service;
using ClinicManagementSystem.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;



namespace ClinicManagementSystem.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class DoctorController : Controller
    {
        private readonly IDoctorService _doctorService;

        public DoctorController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            int? roleId = HttpContext.Session.GetInt32("RoleId");

            if (roleId == null || roleId != 2)   // 2 = Doctor
            {
                context.Result = RedirectToAction("Index", "Login");
                return;
            }

            base.OnActionExecuting(context);
        }

        // ===============================
        // Doctor Index (Now shows today's appointments)
        // ===============================
        public IActionResult Index()
        {
            int? doctorId = HttpContext.Session.GetInt32("DoctorId");

            if (doctorId == null)
                return RedirectToAction("Index", "Login");

            var appointments = _doctorService
                .GetDoctorAppointmentsToday(doctorId.Value);

            return View(appointments);
        }

        [HttpPost]
        [HttpPost]
        public IActionResult AddPrescription(AddPrescriptionVM model)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Index");

            int prescriptionId = _doctorService.AddPrescription(model);

            // 👇 Redirect to Medicine page first
            return RedirectToAction("AddMedicines",
                new { prescriptionId = prescriptionId });
        }

        public IActionResult AddMedicines(int prescriptionId)
        {
            var medicines = _doctorService.GetAvailableMedicines(prescriptionId);

            ViewBag.PrescriptionId = prescriptionId;

            return View(medicines);
        }


        [HttpPost]
        public IActionResult AddPrescriptionMedicine(AddPrescriptionMedicineVM model)
        {
            _doctorService.AddPrescriptionMedicine(model);

            return RedirectToAction("AddMedicines",
                new { prescriptionId = model.PrescriptionId });
        }

        public IActionResult AddLabTests(int prescriptionId)
        {
            var labTests = _doctorService.GetAvailableLabTests(prescriptionId);

            ViewBag.PrescriptionId = prescriptionId;

            return View(labTests);
        }


        [HttpPost]
        public IActionResult AddPrescriptionLabTest(int PrescriptionId, int LabTestId)
        {
            _doctorService.AddPrescriptionLabTest(PrescriptionId, LabTestId);

            return RedirectToAction("AddLabTests",
                new { prescriptionId = PrescriptionId });
        }

        // ===============================
        // Logout
        // ===============================
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Login");
        }
    }
}