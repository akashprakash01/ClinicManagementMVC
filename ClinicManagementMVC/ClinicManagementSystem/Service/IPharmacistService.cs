using ClinicManagementSystem.Models;
using ClinicManagementSystem.ViewModel;

namespace ClinicManagementSystem.Service
{
    public interface IPharmacistService
    {
        // Prescription methods
        public bool DispenseMedicine(int prescriptionMedicineId, int quantity);

        List<PendingPrescriptionVM> GetPendingPrescriptions();
        DispenseViewModel GetDispenseViewModel(int prescriptionId);
        List<PendingPrescriptionMedicineVM> GetPendingPrescriptionMedicines(int prescriptionId);

        // Medicine methods
        List<Medicine> GetAllMedicines();
        bool AddMedicine(Medicine medicine);
        bool UpdateMedicine(Medicine medicine);
        bool DeleteMedicine(int id);
        int GetMedicineStock(int medicineId);

        // Medicine Type methods
        List<MedicineType> GetAllMedicineTypes();
        bool AddMedicineType(MedicineType medicineType);

        // Bill methods
        PharmacyBill GetBillByPrescriptionId(int prescriptionId);
        int CreatePharmacyBill(int prescriptionId, int createdBy);
        BillScreenViewModel GetBillScreenViewModel(int prescriptionId, int billId);
        bool AddBillItem(int pharmacyBillId, int prescriptionMedicineId, int quantity);
        bool RemoveBillItem(int billItemId);
        bool PayBill(int billId);
        FinalBillViewModel GetFinalBillDetails(int billId);
        BillSummaryViewModel GetBillSummary(int billId);
        bool CheckMedicineAvailability(int prescriptionMedicineId, int quantity);
    }
}