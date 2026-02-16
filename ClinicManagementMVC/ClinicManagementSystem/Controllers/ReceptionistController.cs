using ClinicManagementSystem.Models;
using ClinicManagementSystem.Service;
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

        // GET: ReceptionistController/Create
        public ActionResult CreatePatient()
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
                if (ModelState.IsValid)
                {
                    int? employeeId = HttpContext.Session.GetInt32("EmployeeId");

                    if (employeeId == null)
                        return RedirectToAction("Index", "Login");

                    _receptionistService.InsertPatient(patient, employeeId.Value);
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View(patient);
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
        [HttpGet]
        public JsonResult GetSlots(int doctorId, DateTime selectedDate)
        {
            var slots = _receptionistService.GetDoctorSlots(doctorId, selectedDate);
            return Json(slots);
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
