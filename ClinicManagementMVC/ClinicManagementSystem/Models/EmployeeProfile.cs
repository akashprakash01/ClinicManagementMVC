using ClinicManagementSystem.ViewModel;

namespace ClinicManagementSystem.Models
{
    public class EmployeeProfile
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime DOB { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Experience { get; set; }

        public string UserName { get; set; }
        public string Password { get; set; }

        public int RoleId { get; set; }
    }
}
