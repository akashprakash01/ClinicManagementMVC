using System.ComponentModel.DataAnnotations;

namespace ClinicManagementSystem.Models
{
    public class Medicine
    {
        public int MedicineId { get; set; }

        [Required(ErrorMessage = "Medicine Name is required")]
        [StringLength(100)]
        [Display(Name = "Medicine Name")]
        public string MedicineName { get; set; }

        [StringLength(225)]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Medicine Type")]
        public int MedicineTypeId { get; set; }

        [Required]
        [Range(0.01, 999999.99, ErrorMessage = "Price must be greater than 0")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be positive")]
        [Display(Name = "Stock Quantity")]
        public int QuantityStock { get; set; }

        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation properties
        public string MedicineTypeName { get; set; }
    }
}