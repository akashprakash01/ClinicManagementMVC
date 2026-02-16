using ClinicManagementSystem.Models;
using ClinicManagementSystem.Repository;
using ClinicManagementSystem.ViewModel;

namespace ClinicManagementSystem.Service
{
    public class PharmacistService : IPharmacistService
    {
        // Field
        private readonly IPharmacistRepository _pharmacistRepository;

        // Dependency Injection
        public PharmacistService(IPharmacistRepository pharmacistRepository)
        {
            _pharmacistRepository = pharmacistRepository;
        }
        public void AddBillItem(int billId, int prescriptionMedicineId, int quantity)
        {
            if (quantity <= 0)
                throw new Exception("Quantity must be greater than zero");

            _pharmacistRepository.AddBillItem(billId, prescriptionMedicineId, quantity);
        }

        public void AddMedicine(Medicine model)
        {
            if (string.IsNullOrWhiteSpace(model.MedicineName))
                throw new Exception("Medicine name required");

            if (model.Price < 0 || model.QuantityStock < 0)
                throw new Exception("Invalid price or stock");

            _pharmacistRepository.AddMedicine(model);
        }

        public void AddMedicineType(MedicineType model)
        {
            if (string.IsNullOrWhiteSpace(model.TypeName))
                throw new Exception("Type name is required");

            _pharmacistRepository.AddMedicineType(model);
        }

        public int CreatePharmacyBill(int prescriptionId, int createdBy)
        {
            if (prescriptionId <= 0)
                throw new Exception("Invalid Prescription");

            return _pharmacistRepository.CreatePharmacyBill(prescriptionId, createdBy);
        }

        public PharmacyBill GetBillByPrescription(int prescriptionId)
        {
            
            return _pharmacistRepository.GetBillByPrescription(prescriptionId );
        }

        public AddBillItemViewModel GetBillScreen(int prescriptionId, int billId)
        {
            AddBillItemViewModel vm = new AddBillItemViewModel();

            vm.PharmacyBillId = billId;

            vm.PrescriptionMedicines =
                _pharmacistRepository.GetPendingPrescriptionMedicines(prescriptionId);

            vm.BillItems = _pharmacistRepository.GetBillItems(billId);

            vm.GrandTotal = vm.BillItems.Sum(x => x.SubTotal);

            return vm;
        }

        public List<Medicine> GetMedicines()
        {
            return _pharmacistRepository.GetMedicines();
        }

        public List<MedicineType> GetMedicineTypes()
        {
            return _pharmacistRepository.GetMedicineTypes();
        }

        public List<PrescriptionMedicine> GetPendingPrescriptionMedicines(int prescriptionId)
        {
            return _pharmacistRepository.GetPendingPrescriptionMedicines(prescriptionId);
        }

        public void PayBill(int billId)
        {
            _pharmacistRepository.PayBill(billId);
        }
    }
}
