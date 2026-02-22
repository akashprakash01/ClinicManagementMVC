namespace ClinicManagementSystem.Models
{
    public class AddPrescriptionMedicineVM
    {
        public int PrescriptionId { get; set; }
        public int MedicineId { get; set; }
        public string Dosage { get; set; }
        public string Frequency { get; set; }
        public string Duration { get; set; }
        public int Quantity { get; set; }
    }
}
