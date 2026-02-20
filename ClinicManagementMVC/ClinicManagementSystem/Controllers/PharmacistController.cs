using ClinicManagementSystem.Models;
using ClinicManagementSystem.Service;
using ClinicManagementSystem.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using QuestPDF.Drawing;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace ClinicManagementSystem.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]

    public class PharmacistController : Controller
    {
        private readonly IPharmacistService _service;

        public PharmacistController(IPharmacistService service)
        {
            _service = service;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            int? roleId = HttpContext.Session.GetInt32("RoleId");

            if (roleId == null || roleId != 3)   // 3 = Pharmacist
            {
                context.Result = RedirectToAction("Index", "Login");
                return;
            }

            base.OnActionExecuting(context);
        }

        // ================= DASHBOARD / PENDING PRESCRIPTIONS =================
        public IActionResult Index()
        {
            try
            {
                LoadMedicineTypeDropdown();
                var pendingList = _service.GetPendingPrescriptions();
                return View(pendingList);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View(new List<PendingPrescriptionVM>());
            }
        }

        // ================= MEDICINE TYPE MANAGEMENT =================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddMedicineType(MedicineType model)
        {
            try
            {
                int? empId = HttpContext.Session.GetInt32("EmployeeId");

                if (empId == null)
                    throw new Exception("Session expired. Please login again.");

                model.CreatedBy = empId.Value;
                bool result = _service.AddMedicineType(model);

                if (result)
                    TempData["Success"] = "Medicine Type Added Successfully";
                else
                    TempData["Error"] = "Failed to add medicine type";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction("Index");
        }

        public IActionResult MedicineTypeList()
        {
            try
            {
                var data = _service.GetAllMedicineTypes();
                return View(data);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View(new List<MedicineType>());
            }
        }

        // ================= MEDICINE MANAGEMENT =================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddMedicine(Medicine model)
        {
            try
            {
                int? empId = HttpContext.Session.GetInt32("EmployeeId");

                if (empId == null)
                    throw new Exception("Session expired. Please login again.");

                model.CreatedBy = empId.Value;
                bool result = _service.AddMedicine(model);

                if (result)
                    TempData["Success"] = "Medicine Added Successfully";
                else
                    TempData["Error"] = "Failed to add medicine";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction("Index");
        }

        public IActionResult ViewMedicines()
        {
            try
            {
                var medicines = _service.GetAllMedicines();
                LoadMedicineTypeDropdown(); // For the add modal
                return View(medicines);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View(new List<Medicine>());
            }
        }

        [HttpPost]
        public IActionResult EditMedicine(Medicine model)
        {
            try
            {
                bool result = _service.UpdateMedicine(model);
                if (result)
                    TempData["Success"] = "Medicine updated successfully";
                else
                    TempData["Error"] = "Failed to update medicine";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction("ViewMedicines");
        }

        [HttpPost]
        public JsonResult DeleteMedicine(int id)
        {
            try
            {
                bool result = _service.DeleteMedicine(id);
                return Json(new { success = result, message = result ? "Medicine deleted successfully" : "Failed to delete medicine" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // ================= DISPENSE MEDICINE =================

        public IActionResult Dispense(int prescriptionId)
        {
            try
            {
                var model = _service.GetDispenseViewModel(prescriptionId);
                if (model == null)
                {
                    TempData["Error"] = "Prescription not found.";
                    return RedirectToAction("Index");
                }

                // Check if bill exists
                var existingBill = _service.GetBillByPrescriptionId(prescriptionId);
                ViewBag.BillExists = existingBill != null;
                ViewBag.PharmacyBillId = existingBill?.PharmacyBillId;

                return View(model);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public JsonResult DispenseMedicine([FromBody] DispenseMedicineViewModel model)
        {
            try
            {
                if (model == null)
                    return Json(new { success = false, message = "Invalid data received" });

                bool result = _service.DispenseMedicine(
                    model.PrescriptionMedicineId,
                    model.Quantity,
                    model.PharmacyBillId);

                return Json(new { success = result, message = result ? "Medicine dispensed successfully" : "Failed to dispense medicine" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // ================= CREATE BILL =================

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult CreatePharmacyBill(int prescriptionId)
        //{
        //    try
        //    {
        //        int? empId = HttpContext.Session.GetInt32("EmployeeId");

        //        if (empId == null)
        //            throw new Exception("Session expired. Please login again.");

        //        var existingBill = _service.GetBillByPrescriptionId(prescriptionId);

        //        int billId;
        //        if (existingBill != null)
        //        {
        //            billId = existingBill.PharmacyBillId;
        //            TempData["Info"] = "Bill already exists for this prescription.";
        //        }
        //        else
        //        {
        //            billId = _service.CreatePharmacyBill(prescriptionId, empId.Value);
        //            TempData["Success"] = "Bill created successfully. Please add medicine items.";
        //        }

        //        return RedirectToAction("BillScreen", new { prescriptionId, billId });
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["Error"] = ex.Message;
        //        return RedirectToAction("Index");
        //    }
        //}

        // ================= BILL SCREEN (ADD ITEMS) =================

        public IActionResult BillScreen(int prescriptionId)
        {
            try
            {
                int? empId = HttpContext.Session.GetInt32("EmployeeId");

                if (empId == null)
                    throw new Exception("Session expired. Please login again.");

                // 1️⃣ check existing bill
                var existingBill = _service.GetBillByPrescriptionId(prescriptionId);

                int billId;

                // 2️⃣ create bill ONLY if not exists
                if (existingBill == null)
                {
                    billId = _service.CreatePharmacyBill(prescriptionId, empId.Value);
                }
                else
                {
                    billId = existingBill.PharmacyBillId;
                }

                // 3️⃣ load bill screen
                var vm = _service.GetBillScreenViewModel(prescriptionId, billId);

                if (vm == null)
                {
                    TempData["Error"] = "Bill not found.";
                    return RedirectToAction("Index");
                }

                ViewBag.PrescriptionId = prescriptionId;

                return View(vm);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Index");
            }
        }


        // ================= ADD BILL ITEM =================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddBillItem(AddBillItemViewModel model, int prescriptionId)
        {
            try
            {
                bool result = _service.AddBillItem(
                    model.PharmacyBillId,
                    model.PrescriptionMedicineId,
                    model.Quantity);

                if (result)
                    TempData["Success"] = "Medicine added to bill successfully.";
                else
                    TempData["Error"] = "Failed to add medicine to bill.";

                return RedirectToAction("BillScreen", new { prescriptionId });

            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("BillScreen", new { prescriptionId });
            }
        }

        // ================= REMOVE BILL ITEM =================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RemoveBillItem(int billItemId, int pharmacyBillId, int prescriptionId)
        {
            try
            {
                bool result = _service.RemoveBillItem(billItemId);
                if (result)
                    TempData["Success"] = "Item removed from bill.";
                else
                    TempData["Error"] = "Failed to remove item.";

                return RedirectToAction("BillScreen", new { prescriptionId });
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("BillScreen", new { prescriptionId });
            }
        }

        // ================= PAY BILL =================

        public IActionResult PayBill(int billId, int prescriptionId)
        {
            try
            {
                bool result = _service.PayBill(billId);
                if (result)
                    TempData["Success"] = "Payment completed successfully";
                else
                    TempData["Error"] = "Payment failed";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction("BillScreen", new { prescriptionId });
        }

        // ================= GENERATE FINAL BILL =================

        public IActionResult GenerateBill(int billId)
        {
            try
            {
                var bill = _service.GetFinalBillDetails(billId);
                if (bill == null)
                {
                    TempData["Error"] = "Bill not found.";
                    return RedirectToAction("Index");
                }
                return View("FinalBill", bill);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

        // ================= PRINT BILL =================

        public IActionResult PrintBill(int billId)
        {
            try
            {
                var bill = _service.GetFinalBillDetails(billId);
                if (bill == null)
                {
                    TempData["Error"] = "Bill not found.";
                    return RedirectToAction("Index");
                }
                return View(bill);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

        // ================= DOWNLOAD BILL AS PDF =================

        public IActionResult DownloadBillPDF(int billId)
        {
            try
            {
                var bill = _service.GetFinalBillDetails(billId);

                if (bill == null)
                    return NotFound();

                var pdfBytes = Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Margin(30);

                        page.Content().Column(col =>
                        {
                            col.Item().AlignCenter().Text("CLINIC MANAGEMENT SYSTEM")
                                .FontSize(16).Bold();

                            col.Item().AlignCenter().Text("Pharmacy Bill");

                            col.Item().LineHorizontal(1);

                            col.Item().Text($"Bill No : {bill.BillNumber}");
                            col.Item().Text($"Patient : {bill.PatientName}");
                            col.Item().Text($"Doctor  : {bill.DoctorName}");
                            col.Item().Text($"Date    : {bill.BillDate:dd-MM-yyyy}");

                            col.Item().PaddingVertical(10);

                            col.Item().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn(3);
                                    columns.RelativeColumn(2);
                                    columns.RelativeColumn(2);
                                    columns.RelativeColumn(1);
                                    columns.RelativeColumn(2);
                                });

                                table.Header(header =>
                                {
                                    header.Cell().Text("Medicine").Bold();
                                    header.Cell().Text("Dosage").Bold();
                                    header.Cell().Text("Frequency").Bold();
                                    header.Cell().Text("Qty").Bold();
                                    header.Cell().Text("Price").Bold();
                                });

                                foreach (var item in bill.Items)
                                {
                                    table.Cell().Text(item.MedicineName);
                                    table.Cell().Text(item.Dosage);
                                    table.Cell().Text(item.Frequency);
                                    table.Cell().Text(item.Quantity.ToString());
                                    table.Cell().Text($"₹ {item.SubTotal}");
                                }
                            });

                            col.Item().AlignRight().Text($"Total : ₹ {bill.TotalAmount}")
                                .Bold();

                            col.Item().AlignRight().Text(bill.TotalAmountInWords);
                        });
                    });
                }).GeneratePdf();

                return File(pdfBytes, "application/pdf", $"Bill_{bill.BillNumber}.pdf");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Index");
            }
        }


        // ================= HELPER METHODS =================

        private void LoadMedicineTypeDropdown()
        {
            try
            {
                var medicineTypes = _service.GetAllMedicineTypes();
                ViewBag.MedicineTypes = new SelectList(medicineTypes, "MedicineTypeId", "TypeName");
            }
            catch (Exception ex)
            {
                ViewBag.MedicineTypes = new SelectList(new List<MedicineType>());
                TempData["Error"] = "Failed to load medicine types: " + ex.Message;
            }
        }

        // ================= ADDITIONAL ACTIONS =================

        [HttpGet]
        public JsonResult GetMedicineStock(int medicineId)
        {
            try
            {
                int stock = _service.GetMedicineStock(medicineId);
                return Json(new { success = true, stock = stock });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public JsonResult GetPendingMedicines(int prescriptionId)
        {
            try
            {
                var medicines = _service.GetPendingPrescriptionMedicines(prescriptionId);
                return Json(new { success = true, data = medicines });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public JsonResult GetBillSummary(int billId)
        {
            try
            {
                var summary = _service.GetBillSummary(billId);
                return Json(new { success = true, data = summary });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Login");
        }
    }
}