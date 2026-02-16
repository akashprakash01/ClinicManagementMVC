namespace ClinicManagementSystem.Models
{
    public class Medicine
    {
        public int MedicineId { get; set; }
        public string MedicineName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int QuantityStock { get; set; }

        public int MedicineTypeId { get; set; }
        public MedicineType MedicineType { get; set; }
    }
}