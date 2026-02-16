namespace ClinicManagementSystem.ViewModel
{
    public class DoctorAppointmentTodayVM
    {
        public int AppointmentId { get; set; }
        public DateTime AllocatedAt { get; set; }
        public string Token { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public string Contact { get; set; }
        public DateTime AllocatedTimeDate { get; set; }
        public DateTime AllocatedTimeUpTo { get; set; }
    }
}
