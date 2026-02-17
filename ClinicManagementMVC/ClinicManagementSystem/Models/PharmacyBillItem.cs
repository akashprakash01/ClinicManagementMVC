namespace ClinicManagementSystem.Models
{
    public class PharmacyBillItem
    {
        public int PharmacyBillItemId { get; set; }

        public int PharmacyBillId { get; set; }

        public int PrescriptionMedicineId { get; set; }

        public string MedicineName { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal SubTotal { get; set; }
    }
}
