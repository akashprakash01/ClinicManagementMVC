using ClinicManagementSystem.Models;
using ClinicManagementSystem.Service;
using ClinicManagementSystem.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ClinicManagementSystem.Controllers
{
    public class ReceptionistController : Controller
    {
        // Field
        private readonly IReceptionistService _receptionistService;

        // Dependency Injection
        public ReceptionistController(IReceptionistService receptionistService)
        {
            _receptionistService = receptionistService;
        }

        // GET: ReceptionistController
        public IActionResult Index(string contact, string name)
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

            IEnumerable<Patient> patients;

            // If search used
            if (!string.IsNullOrEmpty(contact) || !string.IsNullOrEmpty(name))
                patients = _receptionistService.SelectPatients(contact, name);
            else
                patients = _receptionistService.GetAllPatients();

            return View(patients);
        }

        // GET: ReceptionistController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }


        // POST: ReceptionistController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreatePatient(Patient patient)
        {
            try
            {
                int? employeeId = HttpContext.Session.GetInt32("EmployeeId");

                if (employeeId == null)
                    return Json(new { success = false, message = "Session expired" });

                if (!ModelState.IsValid)
                {
                    var errors = ModelState
                        .Where(x => x.Value.Errors.Count > 0)
                        .ToDictionary(
                            kvp => kvp.Key,
                            kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToList()
                        );

                    return Json(new
                    {
                        success = false,
                        errors
                    });
                }

                _receptionistService.InsertPatient(patient, employeeId.Value);

                return Json(new
                {
                    success = true,
                    message = "Patient added successfully"
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }


        public JsonResult GetAvailableDoctors(int dayOffset)
        {
            var doctors = _receptionistService.SelectAvailableDoctors(dayOffset);
            return Json(doctors);
        }


        [HttpPost]
        public JsonResult BookAppointment(int patientId, int doctorId, DateTime start, DateTime end)
        {
            try
            {
                var result = _receptionistService.InsertAppointment(doctorId, patientId, start, end);

                if (result.Contains("successfully"))
                {
                    return Json(new { success = true, message = result });
                }
                else
                {
                    return Json(new { success = false, message = result });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error booking appointment: " + ex.Message });
            }
        }

        [HttpGet]
        public JsonResult GetSlots(int doctorId, DateTime selectedDate)
        {
            var slots = _receptionistService.GetDoctorSlots(doctorId, selectedDate);
            return Json(slots);
        }


        // GET: ReceptionistController/Edit/5
        public ActionResult Edit(int id)
        {
            int? employeeId = HttpContext.Session.GetInt32("EmployeeId");

            if (employeeId == null)
                return RedirectToAction("Index", "Login");

            var patient = _receptionistService.GetPatientById(id);

            if (patient == null)
                return NotFound();

            return View(patient);
        }

        // POST: ReceptionistController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Patient patient) 
        {
            try
            {
                int? employeeId = HttpContext.Session.GetInt32("EmployeeId");

                if (employeeId == null)
                    return RedirectToAction("Index","Login");

                if (ModelState.IsValid)
                {
                    int rows = _receptionistService.UpdatePatient(patient, employeeId.Value);

                    if (rows > 0)
                    {
                        TempData["SuccessMessage"] = "Patient updated successfully!";
                        return RedirectToAction(nameof(Index));
                    }
                }
                TempData["ErrorMessage"] = "Update failed!";
                return View(patient);
            }
            catch
            {
                return View(patient);
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

        public IActionResult ViewAppointments(
            int? patientId,
            int? doctorId,
            DateTime? appointmentDate,
            DateTime? fromDate,
            DateTime? toDate)
        {
            int? employeeId = HttpContext.Session.GetInt32("EmployeeId");

            if (employeeId == null)
                return RedirectToAction("Index", "Login");

            var list = _receptionistService.ViewAppointments(
                patientId, doctorId, appointmentDate, fromDate, toDate);

            ViewBag.Doctors = _receptionistService.GetDoctors();

            return View(list);
        }

        [HttpPost]
        public IActionResult GenerateBill(int appointmentId)
        {
            var result = _receptionistService.CreatePatientBill(appointmentId);

            if (!result.IsSuccess)
                return Json(result);

            var bill = _receptionistService.GetBillById(result.PatientBillId);

            return Json(bill);
        }


        public IActionResult BillDetails(int id)
        {
            var bill = _receptionistService.GetBillById(id);

            if (bill == null)
                return NotFound();

            return View(bill);
        }

        [HttpGet]
        public IActionResult GetBillById(int id)
        {
            var bill = _receptionistService.GetBillById(id);
            return Json(bill);
        }


    }
}
