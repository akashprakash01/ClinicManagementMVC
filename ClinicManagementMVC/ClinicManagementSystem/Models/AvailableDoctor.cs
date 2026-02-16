namespace ClinicManagementSystem.Models
{
    public class AvailableDoctor
    {
        public int DoctorId { get; set; }
        public string DoctorName { get; set; }
        public string SpecializationName { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
}
