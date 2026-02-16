namespace ClinicManagementSystem.ViewModel
{
    public class PatientBillViewModel
    {
        public int AppointmentId { get; set; }
        public int PatientBillId { get; set; }
        public decimal BillAmount { get; set; }
        public string BillStatus { get; set; }
        public string StatusMessage { get; set; }
        public bool IsSuccess { get; set; }

        // Display properties
        public string PatientName { get; set; }
        public string DoctorName { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Token { get; set; }
    }
}
