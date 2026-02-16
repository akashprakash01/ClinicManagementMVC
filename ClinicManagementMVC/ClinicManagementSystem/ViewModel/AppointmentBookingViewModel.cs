using System.ComponentModel.DataAnnotations;

namespace ClinicManagementSystem.ViewModel
{
    public class AppointmentBookingViewModel
    {
        [Required(ErrorMessage ="Please select a doctor.")]
        [Display(Name ="Doctor")]
        public int DoctorId  { get; set; }

        [Required(ErrorMessage ="Please select a patient.")]
        [Display(Name ="Patient")]
        public int PatientId { get; set; }

        [Required(ErrorMessage = "Please select appointment date and time")]
        [Display(Name = "Appointment Date & Time")]
        [DataType(DataType.DateTime)]
        public DateTime AllocatedTimeDate { get; set; }


        // Display properties
        public string PatientName { get; set; }
        public string PatientContact { get; set; }
        public string DoctorName { get; set; }

        // Result properties
        public string Token { get; set; }
        public DateTime AllocatedTimeUpTo { get; set; }
        public int AppointmentId { get; set; }
        public string StatusMessage { get; set; }
        public bool IsSuccess { get; set; }
    }
}






