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
        [StringLength(50, MinimumLength = 3,
            ErrorMessage = "Name must be between 3 and 50 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage ="Date of Birth is required.")]
        [DataType(DataType.Date)]
        [Display(Name ="Date of Birth")]
        [CustomValidation(typeof(Patient), nameof(ValidateDOB))]
        public DateTime DOB { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        [RegularExpression("Male|Female",
            ErrorMessage = "Invalid gender selection.")]
        public string Gender { get; set; }

        [Required(ErrorMessage ="Contact No is required.")]
        [RegularExpression(@"^[6-9]\d{9}$",
            ErrorMessage ="Invalid number. Must start with 6,7,8,9")]
        
        public string Contact { get; set; }

        [Required(ErrorMessage ="Address is required.")]
        [StringLength(250, ErrorMessage = "Address cannot exceed 250 characters.")]
        public string Address { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }



        //  Custom DOB validation
        public static ValidationResult ValidateDOB(DateTime dob, ValidationContext context)
        {
            if (dob >= DateTime.Today)
            {
                return new ValidationResult("DOB must be a past date.");
            }

            if (dob < DateTime.Today.AddYears(-120))
            {
                return new ValidationResult("Enter a valid DOB.");
            }

            return ValidationResult.Success;
        }
    }
}
