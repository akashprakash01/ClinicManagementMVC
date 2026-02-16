namespace ClinicManagementSystem.Models
{
    public class Test_PrescriptionMedicines
    {
        public int PrescriptionMedicineId { get; set; }
        public int PrescriptionId { get; set; }
        public int MedicineId { get; set; }
        public string Dosage { get; set; }
        public string Frequency { get; set; }
        public string Duration { get; set; }
        public string Instructions { get; set; }
        public bool Status { get; set; }
    }
}
