namespace ClinicManagementSystem.Models
{
    public class MedicineVM
    {
        public int MedicineId { get; set; }
        public string MedicineName { get; set; }
        public string Description { get; set; }
    }

    public class AddPrescriptionMedicineVM
    {
        public int PrescriptionId { get; set; }
        public int MedicineId { get; set; }
        public string Dosage { get; set; }
        public string Frequency { get; set; }
        public string Duration { get; set; }
    }
}
