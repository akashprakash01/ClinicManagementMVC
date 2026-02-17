using ClinicManagementSystem.Models;
using ClinicManagementSystem.ViewModel;

namespace ClinicManagementSystem.Service
{
    public interface IPharmacistService
    {
        // Medicine Type
        void AddMedicineType(MedicineType model);
        List<MedicineType> GetMedicineTypes();

        // Medicine
        void AddMedicine(Medicine model);
        List<Medicine> GetMedicines();

        // Billing
        int CreatePharmacyBill(int prescriptionId, int createdBy);
        PharmacyBill GetBillByPrescription(int prescriptionId);
        AddBillItemViewModel GetBillScreen(int prescriptionId, int billId);

        void AddBillItem(int billId, int prescriptionMedicineId, int quantity);

        void PayBill(int billId);

        // Prescription Medicines
        List<PrescriptionMedicine> GetPendingPrescriptionMedicines(int prescriptionId);
    }
}
