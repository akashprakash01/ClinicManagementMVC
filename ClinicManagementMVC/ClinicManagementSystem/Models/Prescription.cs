namespace ClinicManagementSystem.Models
{
    public class Prescription
    {
        public int PrescriptionId { get; set; }
        public string Symptoms { get; set; }
        public string Diagnosis { get; set; }
        public string Advice { get; set; }
        public string PatientNotes { get; set; }
        public DateTime FollowUpDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public int AppointmentId { get; set; }
        public int DoctorId { get; set; }
    }
}
