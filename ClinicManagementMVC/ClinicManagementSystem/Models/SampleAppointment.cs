namespace ClinicManagementSystem.Models
{
    public class SampleAppointment
    {
        public int AppointmentId { get; set; }
        public DateTime Allocated_at { get; set; }
        public int DoctorId { get; set; }
        public int PatientId { get; set; }
        public bool PatientCheckup { get; set; }
        public string Token { get; set; }
        public DateTime AllocatedTimeDate { get; set; }
        public DateTime AllocatedTimeUpto { get; set; }


    }
}
