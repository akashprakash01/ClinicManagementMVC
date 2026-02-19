namespace ClinicManagementSystem.Models
{
    public class PharmacyBill
    {
        public int PharmacyBillId { get; set; }
        public int PrescriptionId { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime BillDate { get; set; }
        public int PaymentStatus { get; set; } // 0 = Unpaid, 1 = Paid

        // Navigation properties
        public string PatientName { get; set; }
        public string DoctorName { get; set; }
        public DateTime PrescriptionDate { get; set; }
        public List<PharmacyBillItem> BillItems { get; set; }
    }
}
