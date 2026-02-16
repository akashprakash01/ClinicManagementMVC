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

        public IActionResult Index()
        {
            return View();
        }

        // ================= MEDICINE TYPE =================

        public IActionResult AddMedicineType()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddMedicineType(MedicineType model)
        {
            try
            {
                model.CreatedBy = 1; // get from session later

                _service.AddMedicineType(model);

                TempData["Success"] = "Medicine Type Added Successfully";
                return RedirectToAction("MedicineTypeList");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View(model);
            }
        }

        public IActionResult MedicineTypeList()
        {
            var data = _service.GetMedicineTypes();
            return View(data);
        }

        // ================= ADD MEDICINE =================

        public IActionResult AddMedicine()
        {
            LoadMedicineTypeDropdown();
            return View();
        }

        [HttpPost]
        public IActionResult AddMedicine(Medicine model)
        {
            try
            {
                model.CreatedBy = 1;

                _service.AddMedicine(model);

                TempData["Success"] = "Medicine Added Successfully";
                return RedirectToAction("ViewMedicines");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                LoadMedicineTypeDropdown();
                return View(model);
            }
        }

        // ================= VIEW MEDICINES =================

        public IActionResult ViewMedicines()
        {
            var medicines = _service.GetMedicines();
            return View(medicines);
        }

        // ================= CREATE BILL =================

        public IActionResult CreatePharmacyBill()
        {
            return View();
        }

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
                return View(model);
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

                return RedirectToAction("BillScreen", new
                {
                    prescriptionId = prescriptionId,
                    billId = billId
                });
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;

                return RedirectToAction("BillScreen", new
                {
                    prescriptionId = prescriptionId,
                    billId = billId
                });
            }
        }

        // ================= DROPDOWN HELPER =================

        private void LoadMedicineTypeDropdown()
        {
            ViewBag.MedicineTypes = new SelectList(
                _service.GetMedicineTypes(),
                "MedicineTypeId",
                "TypeName"
            );
        }
    }
}
