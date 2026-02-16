using NuGet.Common;
using System.Security.Principal;

namespace ClinicManagementSystem.Models
{
    public class Appointment
    {
        public int AppointmentId { get; set; }
        public int DoctorId { get; set; }
        public int PatientId { get; set; }
        public string Token { get; set; }
        public DateTime AllocatedTimeDate { get; set; }
        public DateTime AllocatedTimeUpTo { get; set; }
        public DateTime AllocatedAt { get; set; }
        public bool PatientCheckup { get; set; }
        public string DoctorName { get; set; }
        public string PatientName { get; set; }

    }
}



