using ClinicManagementSystem.Models;
using ClinicManagementSystem.ViewModel;

namespace ClinicManagementSystem.Repository
{
    public interface IPharmacistRepository
    {
        // Prescription methods
        IEnumerable<Prescription> GetPendingPrescriptions();
        Prescription GetPrescriptionById(int id);
        int GetPendingMedicineCount(int prescriptionId);
        List<PrescriptionMedicine> GetPrescriptionMedicines(int prescriptionId);
        IEnumerable<Medicine> GetAvailableMedicinesForPrescription(int prescriptionId);

        // Medicine methods
        IEnumerable<Medicine> GetAllMedicines();
        Medicine GetMedicineById(int id);
        int AddMedicine(Medicine medicine);
        bool UpdateMedicine(Medicine medicine);
        bool DeleteMedicine(int id);

        // Medicine Type methods
        IEnumerable<MedicineType> GetAllMedicineTypes();
        int AddMedicineType(MedicineType medicineType);

        // Bill methods
        PharmacyBill GetPharmacyBill(int prescriptionId);
        PharmacyBill GetPharmacyBillById(int billId);
        int CreatePharmacyBill(int prescriptionId, int createdBy);
        bool AddPharmacyBillItem(int pharmacyBillId, int prescriptionMedicineId, int quantity);
        bool RemovePharmacyBillItem(int billItemId);
        List<PharmacyBillItem> GetPharmacyBillItems(int pharmacyBillId);
        bool PayPharmacyBill(int pharmacyBillId);

        // Dispense workflow
        bool CheckStock(int prescriptionMedicineId, int quantity);
        bool UpdateDispenseStatus(int prescriptionMedicineId, int quantity);
        bool ReduceStock(int prescriptionMedicineId, int quantity);

    }

}

