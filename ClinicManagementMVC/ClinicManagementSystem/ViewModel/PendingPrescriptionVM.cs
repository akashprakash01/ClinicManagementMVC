namespace ClinicManagementSystem.ViewModel
{
    public class PendingPrescriptionVM
    {
        public int PrescriptionId { get; set; }
        public string PatientName { get; set; }
        public string DoctorName { get; set; }
        public DateTime PrescriptionDate { get; set; }
        public int PendingCount { get; set; }
    }
}
