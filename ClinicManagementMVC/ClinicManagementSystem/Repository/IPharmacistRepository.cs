using ClinicManagementSystem.Models;

namespace ClinicManagementSystem.Repository
{
    public interface IPharmacistRepository
    {
        // Medicine Type
        void AddMedicineType(MedicineType model);
        List<MedicineType> GetMedicineTypes();

        // Medicine
        void AddMedicine(Medicine model);
        List<Medicine> GetMedicines();

        // Bill
        int CreatePharmacyBill(int prescriptionId, int createdBy);
        PharmacyBill GetBillByPrescription(int prescriptionId);
        List<PharmacyBillItem> GetBillItems(int billId);

        void AddBillItem(int billId, int prescriptionMedicineId, int quantity);
        void PayBill(int billId);

        // Prescription Medicines
        List<PrescriptionMedicine> GetPendingPrescriptionMedicines(int prescriptionId);
    }
}
