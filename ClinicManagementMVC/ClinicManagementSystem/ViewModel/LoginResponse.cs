namespace ClinicManagementSystem.ViewModel
{
    public class LoginResponse
    {
        public bool IsSuccess { get; set; }
        public int? EmployeeId { get; set; }
        public int? DoctorId { get; set; }
        public int? RoleId { get; set; }
    }
}
