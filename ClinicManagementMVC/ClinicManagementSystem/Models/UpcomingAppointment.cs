namespace ClinicManagementSystem.Models
{
    public class UpcomingAppointment
    {
        public int AppointmentId { get; set; }
        public string PatientName { get; set; }
        public string DoctorName { get; set; }
        public DateTime Time { get; set; }
        public string Token { get; set; }
    }
}
