using ClinicManagementSystem.Models;
using ClinicManagementSystem.ViewModel;

namespace ClinicManagementSystem.Service
{
    public interface IUserServic
    {
        LoginResponse AuthenticateUserNameAndPassword(string username, string password);
    }
}
