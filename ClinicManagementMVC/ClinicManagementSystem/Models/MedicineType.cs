namespace ClinicManagementSystem.Models
{
    public class MedicineType
    {
        public int MedicineTypeId { get; set; }

        public string TypeName { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }

    }
}