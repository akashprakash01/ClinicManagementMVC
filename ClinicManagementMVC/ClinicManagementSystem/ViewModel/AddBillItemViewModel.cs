using ClinicManagementSystem.Models;

namespace ClinicManagementSystem.ViewModel
{
    public class AddBillItemViewModel
    {
        public int PharmacyBillId { get; set; }

        public int PrescriptionMedicineId { get; set; }

        public int Quantity { get; set; }

        public List<PrescriptionMedicine> PrescriptionMedicines { get; set; }

        public List<PharmacyBillItem> BillItems { get; set; }

        public decimal GrandTotal { get; set; }
    }
}
