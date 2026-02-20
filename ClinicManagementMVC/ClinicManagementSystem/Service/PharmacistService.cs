using ClinicManagementSystem.Models;
using ClinicManagementSystem.Repository;
using ClinicManagementSystem.ViewModel;

namespace ClinicManagementSystem.Service
{
    public class PharmacistService : IPharmacistService
    {
        private readonly IPharmacistRepository _repository;

        public PharmacistService(IPharmacistRepository repository)
        {
            _repository = repository;
        }

        // ================= PRESCRIPTION METHODS =================

        public List<PendingPrescriptionVM> GetPendingPrescriptions()
        {
            try
            {
                var prescriptions = _repository.GetPendingPrescriptions();
                var pendingList = new List<PendingPrescriptionVM>();

                foreach (var item in prescriptions)
                {
                    var pendingCount = _repository.GetPendingMedicineCount(item.PrescriptionId);

                    pendingList.Add(new PendingPrescriptionVM
                    {
                        PrescriptionId = item.PrescriptionId,
                        PatientName = item.PatientName,
                        DoctorName = item.DoctorName,
                        PrescriptionDate = item.PrescriptionDate,
                        PendingCount = pendingCount
                    });
                }

                return pendingList;
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching pending prescriptions: " + ex.Message);
            }
        }

        public DispenseViewModel GetDispenseViewModel(int prescriptionId)
        {
            try
            {
                var prescription = _repository.GetPrescriptionById(prescriptionId);
                if (prescription == null)
                    return null;

                var model = new DispenseViewModel
                {
                    PrescriptionId = prescription.PrescriptionId,
                    PatientName = prescription.PatientName,
                    DoctorName = prescription.DoctorName,
                    PrescribedMedicines = _repository.GetPrescriptionMedicines(prescriptionId),
                    AvailableMedicines = _repository.GetAvailableMedicinesForPrescription(prescriptionId).ToList()
                };

                return model;
            }
            catch (Exception ex)
            {
                throw new Exception("Error loading dispense view: " + ex.Message);
            }
        }

        public List<PendingPrescriptionMedicineVM> GetPendingPrescriptionMedicines(int prescriptionId)
        {
            try
            {
                var medicines = _repository.GetPrescriptionMedicines(prescriptionId);

                return medicines.Where(m => m.Status == 1).Select(m => new PendingPrescriptionMedicineVM
                {
                    PrescriptionMedicineId = m.PrescriptionMedicineId,
                    MedicineName = m.MedicineName,
                    Dosage = m.Dosage,
                    Frequency = m.Frequency,
                    Quantity = m.Quantity,
                    Price = m.Price
                    // DisplayText will be automatically computed by the property getter
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching pending medicines: " + ex.Message);
            }
        }

        // ================= MEDICINE METHODS =================

        public List<Medicine> GetAllMedicines()
        {
            try
            {
                return _repository.GetAllMedicines().ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching medicines: " + ex.Message);
            }
        }

        public bool AddMedicine(Medicine medicine)
        {
            try
            {
                int result = _repository.AddMedicine(medicine);
                return result > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding medicine: " + ex.Message);
            }
        }

        public bool UpdateMedicine(Medicine medicine)
        {
            try
            {
                return _repository.UpdateMedicine(medicine);
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating medicine: " + ex.Message);
            }
        }

        public bool DeleteMedicine(int id)
        {
            try
            {
                return _repository.DeleteMedicine(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting medicine: " + ex.Message);
            }
        }

        public int GetMedicineStock(int medicineId)
        {
            try
            {
                var medicine = _repository.GetMedicineById(medicineId);
                return medicine?.QuantityStock ?? 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching medicine stock: " + ex.Message);
            }
        }

        // ================= MEDICINE TYPE METHODS =================

        public List<MedicineType> GetAllMedicineTypes()
        {
            try
            {
                return _repository.GetAllMedicineTypes().ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching medicine types: " + ex.Message);
            }
        }

        public bool AddMedicineType(MedicineType medicineType)
        {
            try
            {
                int result = _repository.AddMedicineType(medicineType);
                return result > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding medicine type: " + ex.Message);
            }
        }

        // ================= BILL METHODS =================

        public PharmacyBill GetBillByPrescriptionId(int prescriptionId)
        {
            try
            {
                return _repository.GetPharmacyBill(prescriptionId);
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching bill: " + ex.Message);
            }
        }

        public int CreatePharmacyBill(int prescriptionId, int createdBy)
        {
            try
            {
                int billId = _repository.CreatePharmacyBill(prescriptionId, createdBy);

                // ⭐ AUTO ADD DISPENSED MEDICINES
                _repository.AddDispensedMedicinesToBill(prescriptionId, billId);

                return billId;
            }

            catch (Exception ex)
            {
                throw new Exception("Error creating bill: " + ex.Message);
            }
        }

        public BillScreenViewModel GetBillScreenViewModel(int prescriptionId, int billId)
        {
            try
            {
                var bill = _repository.GetPharmacyBill(billId);
                if (bill == null)
                    return null;

                var pendingMedicines = GetPendingPrescriptionMedicines(prescriptionId);
                var billItems = _repository.GetPharmacyBillItems(billId);

                var viewModel = new BillScreenViewModel
                {
                    Bill = bill,
                    PendingMedicines = pendingMedicines, // Now this is List<PendingPrescriptionMedicineVM>
                    BillItems = billItems,
                    TotalAmount = billItems.Sum(i => i.SubTotal)
                };

                return viewModel;
            }
            catch (Exception ex)
            {
                throw new Exception("Error loading bill screen: " + ex.Message);
            }
        }

        public bool AddBillItem(int pharmacyBillId, int prescriptionMedicineId, int quantity)
        {
            try
            {
                return _repository.AddPharmacyBillItem(pharmacyBillId, prescriptionMedicineId, quantity);
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding bill item: " + ex.Message);
            }
        }

        public bool RemoveBillItem(int billItemId)
        {
            try
            {
                return _repository.RemovePharmacyBillItem(billItemId);
            }
            catch (Exception ex)
            {
                throw new Exception("Error removing bill item: " + ex.Message);
            }
        }

        public bool PayBill(int billId)
        {
            try
            {
                return _repository.PayPharmacyBill(billId);
            }
            catch (Exception ex)
            {
                throw new Exception("Error processing payment: " + ex.Message);
            }
        }

        public FinalBillViewModel GetFinalBillDetails(int billId)
        {
            try
            {
                var bill = _repository.GetPharmacyBillById(billId);
                if (bill == null)
                    return null;

                var billItems = _repository.GetPharmacyBillItems(billId);
                var prescription = _repository.GetPrescriptionById(bill.PrescriptionId);

                var viewModel = new FinalBillViewModel
                {
                    PharmacyBillId = bill.PharmacyBillId,
                    BillNumber = "BILL" + bill.PharmacyBillId.ToString("D6"),
                    PrescriptionId = bill.PrescriptionId,
                    PatientName = bill.PatientName,
                    DoctorName = bill.DoctorName,
                    BillDate = bill.BillDate,
                    PrescriptionDate = prescription?.PrescriptionDate ?? DateTime.Now,
                    PaymentStatus = bill.PaymentStatus,
                    Items = billItems.Select(i => new BillItemViewModel
                    {
                        MedicineName = i.MedicineName,
                        Dosage = i.Dosage,
                        Frequency = i.Frequency,
                        Quantity = i.Quantity,
                        UnitPrice = i.UnitPrice
                    }).ToList()
                };

                viewModel.TotalAmount = viewModel.Items.Sum(i => i.SubTotal);
                viewModel.TotalAmountInWords = NumberToWords(viewModel.TotalAmount);

                return viewModel;
            }
            catch (Exception ex)
            {
                throw new Exception("Error generating final bill: " + ex.Message);
            }
        }

        public BillSummaryViewModel GetBillSummary(int billId)
        {
            try
            {
                var billItems = _repository.GetPharmacyBillItems(billId);

                return new BillSummaryViewModel
                {
                    TotalItems = billItems.Count,
                    SubTotal = billItems.Sum(i => i.SubTotal),
                    Tax = 0, // Add tax calculation if needed
                    Discount = 0, // Add discount calculation if needed
                    GrandTotal = billItems.Sum(i => i.SubTotal)
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching bill summary: " + ex.Message);
            }
        }

        public bool DispenseMedicine(int prescriptionMedicineId, int quantity, int pharmacyBillId)
        {
            try
            {
                if (quantity <= 0)
                    throw new Exception("Invalid quantity");

                // 🔒 Check stock
                if (!_repository.CheckStock(prescriptionMedicineId, quantity))
                    throw new Exception("Insufficient stock");

                // 🔹 Reduce stock
                bool stockReduced = _repository.ReduceStock(prescriptionMedicineId, quantity);

                if (!stockReduced)
                    throw new Exception("Failed to reduce stock");

                // 🔹 Update dispense status
                bool dispensed = _repository.UpdateDispenseStatus(prescriptionMedicineId, quantity);

                if (!dispensed)
                    throw new Exception("Failed to update dispense status");

                // 🔹 Add to bill ONLY if bill already exists
                if (pharmacyBillId > 0)
                {
                    bool billItemAdded = _repository.AddPharmacyBillItem(
                        pharmacyBillId,
                        prescriptionMedicineId,
                        quantity);

                    if (!billItemAdded)
                        throw new Exception("Failed to add item to bill");
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error dispensing medicine: " + ex.Message);
            }
        }





        public bool CheckMedicineAvailability(int prescriptionMedicineId, int quantity)
        {
            try
            {
                var medicines = _repository.GetPrescriptionMedicines(0); // You might need to modify this
                var medicine = medicines.FirstOrDefault(m => m.PrescriptionMedicineId == prescriptionMedicineId);

                if (medicine == null)
                    return false;

                var stock = GetMedicineStock(medicine.MedicineId);
                return stock >= quantity && medicine.Quantity >= quantity;
            }
            catch (Exception ex)
            {
                throw new Exception("Error checking medicine availability: " + ex.Message);
            }
        }

        // ================= HELPER METHODS =================

        private string NumberToWords(decimal number)
        {
            if (number == 0)
                return "Zero";

            int num = (int)Math.Floor(number);
            int paisa = (int)((number - num) * 100);

            string words = ConvertNumberToWords(num) + " Rupees";

            if (paisa > 0)
            {
                words += " and " + ConvertNumberToWords(paisa) + " Paisa";
            }

            return words + " Only";
        }

        private string ConvertNumberToWords(int number)
        {
            if (number == 0)
                return "";

            if (number < 0)
                return "Minus " + ConvertNumberToWords(Math.Abs(number));

            string words = "";

            if ((number / 10000000) > 0)
            {
                words += ConvertNumberToWords(number / 10000000) + " Crore ";
                number %= 10000000;
            }

            if ((number / 100000) > 0)
            {
                words += ConvertNumberToWords(number / 100000) + " Lakh ";
                number %= 100000;
            }

            if ((number / 1000) > 0)
            {
                words += ConvertNumberToWords(number / 1000) + " Thousand ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += ConvertNumberToWords(number / 100) + " Hundred ";
                number %= 100;
            }

            if (number > 0)
            {
                if (words != "")
                    words += "and ";

                var units = new[] { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
                var tens = new[] { "Zero", "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };

                if (number < 20)
                    words += units[number];
                else
                {
                    words += tens[number / 10];
                    if ((number % 10) > 0)
                        words += " " + units[number % 10];
                }
            }

            return words.Trim();
        }
    }
}