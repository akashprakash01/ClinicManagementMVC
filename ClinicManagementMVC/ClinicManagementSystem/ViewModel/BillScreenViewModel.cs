using ClinicManagementSystem.Models;

namespace ClinicManagementSystem.ViewModel
{
    public class BillScreenViewModel
    {
        public PharmacyBill Bill { get; set; }
        public List<PendingPrescriptionMedicineVM> PendingMedicines { get; set; }
        public List<PharmacyBillItem> BillItems { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
