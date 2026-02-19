namespace ClinicManagementSystem.Models
{
    public class PrescriptionMedicineView
    {
        public int PrescriptionId { get; set; }
        public DateTime PrescriptionDate { get; set; }

        public int PatientId { get; set; }
        public string PatientName { get; set; }

        public int DoctorId { get; set; }
        public string DoctorName { get; set; }

        public string MedicineName { get; set; }
        public string Dosage { get; set; }
        public string Frequency { get; set; }
        public string Duration { get; set; }
    }
}
