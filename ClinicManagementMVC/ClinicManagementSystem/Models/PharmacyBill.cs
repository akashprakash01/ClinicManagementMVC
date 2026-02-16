namespace ClinicManagementSystem.Models
{
    public class PharmacyBill
    {
        public int PharmacyBillId { get; set; }

        public int PrescriptionId { get; set; }

        public decimal TotalAmount { get; set; }

        public DateTime BillDate { get; set; }

        public bool PaymentStatus { get; set; }
    }
}
