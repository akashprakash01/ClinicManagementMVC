using System.ComponentModel.DataAnnotations;

namespace ClinicManagementSystem.Models
{
    public class MedicineType
    {
        public int MedicineTypeId { get; set; }

        [Required(ErrorMessage = "Type Name is required")]
        [StringLength(50)]
        [Display(Name = "Type Name")]
        public string TypeName { get; set; }

        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}