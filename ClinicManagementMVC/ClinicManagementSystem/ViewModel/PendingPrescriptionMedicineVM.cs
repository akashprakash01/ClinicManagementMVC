namespace ClinicManagementSystem.ViewModel
{
    public class PendingPrescriptionMedicineVM
    {
        public int PrescriptionMedicineId { get; set; }
        public string MedicineName { get; set; }
        public string Dosage { get; set; }
        public string Frequency { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string DisplayText => $"{MedicineName} - {Dosage} (Qty: {Quantity})";
    }
}
