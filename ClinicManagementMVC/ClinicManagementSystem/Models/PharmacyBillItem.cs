namespace ClinicManagementSystem.Models
{
    public class PharmacyBillItem
    {
        public int PharmacyBillItemId { get; set; }
        public int PharmacyBillId { get; set; }
        public int PrescriptionMedicineId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        // Computed property
        public decimal SubTotal => Quantity * UnitPrice;

        // Navigation properties
        public string MedicineName { get; set; }
        public string Dosage { get; set; }
        public string Frequency { get; set; }
    }
}
