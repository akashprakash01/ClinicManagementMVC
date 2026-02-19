namespace ClinicManagementSystem.ViewModel
{
    public class FinalBillViewModel
    {
        public int PharmacyBillId { get; set; }
        public string BillNumber { get; set; }
        public int PrescriptionId { get; set; }
        public string PatientName { get; set; }
        public string DoctorName { get; set; }
        public DateTime BillDate { get; set; }
        public DateTime PrescriptionDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string TotalAmountInWords { get; set; }
        public int PaymentStatus { get; set; }
        public string PaymentStatusText => PaymentStatus == 1 ? "Paid" : "Unpaid";
        public List<BillItemViewModel> Items { get; set; }
    }
}
