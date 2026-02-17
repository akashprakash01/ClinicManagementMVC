using ClinicManagementSystem.Models;
using ClinicManagementSystem.Service;
using ClinicManagementSystem.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ClinicManagementSystem.Controllers
{
    public class PharmacistController : Controller
    {
        private readonly IPharmacistService _service;

        public PharmacistController(IPharmacistService service)
        {
            _service = service;
        }

        // ================= DASHBOARD =================

        public IActionResult Index()
        {
            LoadMedicineTypeDropdown();
            return View();
        }

        // ================= MEDICINE TYPE =================

        [HttpPost]
        public IActionResult AddMedicineType(MedicineType model)
        {
            try
            {
                model.CreatedBy = HttpContext.Session.GetInt32("EmployeeId").Value;


                _service.AddMedicineType(model);

                TempData["Success"] = "Medicine Type Added Successfully";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction("Index");
        }

        public IActionResult MedicineTypeList()
        {
            var data = _service.GetMedicineTypes();
            return View(data);
        }

        // ================= ADD MEDICINE =================

        [HttpPost]
        public IActionResult AddMedicine(Medicine model)
        {
            try
            {
                model.CreatedBy = HttpContext.Session.GetInt32("EmployeeId").Value;

                _service.AddMedicine(model);

                TempData["Success"] = "Medicine Added Successfully";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction("Index");
        }

        // ================= VIEW MEDICINES =================

        public IActionResult ViewMedicines()
        {
            var medicines = _service.GetMedicines();
            return View(medicines);
        }

        // ================= CREATE BILL =================

        [HttpPost]
        public IActionResult CreatePharmacyBill(CreatePharmacyBill model)
        {
            try
            {
                int billId = _service.CreatePharmacyBill(model.PrescriptionId, 1);

                return RedirectToAction("BillScreen", new
                {
                    prescriptionId = model.PrescriptionId,
                    billId = billId
                });
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

        // ================= BILL SCREEN =================

        public IActionResult BillScreen(int prescriptionId, int billId)
        {
            var vm = _service.GetBillScreen(prescriptionId, billId);
            ViewBag.PrescriptionId = prescriptionId;

            return View(vm);
        }

        // ================= ADD BILL ITEM =================

        [HttpPost]
        public IActionResult AddBillItem(AddBillItemViewModel model, int prescriptionId)
        {
            try
            {
                _service.AddBillItem(
                    model.PharmacyBillId,
                    model.PrescriptionMedicineId,
                    model.Quantity);

                return RedirectToAction("BillScreen", new
                {
                    prescriptionId = prescriptionId,
                    billId = model.PharmacyBillId
                });
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;

                return RedirectToAction("BillScreen", new
                {
                    prescriptionId = prescriptionId,
                    billId = model.PharmacyBillId
                });
            }
        }

        // ================= PAY BILL =================

        public IActionResult PayBill(int billId, int prescriptionId)
        {
            try
            {
                _service.PayBill(billId);

                TempData["Success"] = "Payment completed successfully";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction("BillScreen", new
            {
                prescriptionId = prescriptionId,
                billId = billId
            });
        }

        // ================= DROPDOWN =================

        private void LoadMedicineTypeDropdown()
        {
            ViewBag.MedicineTypes = new SelectList(
                _service.GetMedicineTypes(),
                "MedicineTypeId",
                "TypeName");
        }
    }
}
