using System.ComponentModel.DataAnnotations;

namespace ClinicManagementSystem.Models
{
    public class PrescriptionMedicine
    {
        public int PrescriptionMedicineId { get; set; }
        public int PrescriptionId { get; set; }
        public int MedicineId { get; set; }

        [Required]
        [StringLength(50)]
        public string Dosage { get; set; }

        [Required]
        [StringLength(50)]
        public string Frequency { get; set; }

        [Required]
        [StringLength(50)]
        public string Duration { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        public int Status { get; set; } // 1 = Pending, 0 = Dispensed

        // Navigation properties
        public string MedicineName { get; set; }
        public decimal Price { get; set; }
    }
}
