namespace ClinicManagementSystem.ViewModel
{
    public class AppointmentViewModel
    {
        public int AppointmentId { get; set; }
        public DateTime BookedAt { get; set; }
        public int DoctorId { get; set; }
        public string DoctorName { get; set; }
        public string SpecializationName { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public bool? PatientCheckup { get; set; }
        public string Token { get; set; }
        public DateTime AppointmentDate { get; set; }
        public DateTime AppointmentEndTime { get; set; }
        public string AppointmentStatus { get; set; }
        public bool IsBillGenerated { get; set; }
        public int? PatientBillId { get; set; }
    }

}
