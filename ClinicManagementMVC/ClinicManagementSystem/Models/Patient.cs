using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ClinicManagementSystem.Models
{
    public class Patient
    {
        [Key]
        public int PatientId { get; set; }

        [Required(ErrorMessage ="Name is required.")]
        [RegularExpression(@"^[A-Za-z][A-Za-z\s]+$",
            ErrorMessage = "Name must contain only letters and spaces.")]
        public string Name { get; set; }

        [Required(ErrorMessage ="Date of Birth is required.")]
        [BindProperty,DataType(DataType.Date)]
        [Display(Name ="Date of Birth")]
        public DateTime DOB { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        [StringLength(10)]
        public string Gender { get; set; }

        [Required(ErrorMessage ="Contact No is required.")]
        [RegularExpression(@"^[6-9]\d{9}$",
            ErrorMessage ="Invalid number. Must start with 6,7,8,9")]
        [StringLength(10)]
        public string Contact { get; set; }

        [Required(ErrorMessage ="Address is required.")]
        [StringLength(150)]
        public string Address { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
