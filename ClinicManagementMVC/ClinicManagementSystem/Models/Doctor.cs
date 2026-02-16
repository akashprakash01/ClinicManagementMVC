namespace ClinicManagementSystem.Models
{
    public class Doctor
    {
        public int DoctorId { get; set; }
        public decimal Fees { get; set; }
        public int EmployeeId { get; set; }
        public int SpecializationId { get; set; }
    }
}
