using ClinicManagementSystem.Models;
using ClinicManagementSystem.ViewModel;

namespace ClinicManagementSystem.Repository
{
    public interface IUserRepository
    {
        LoginResponse Athentication(string username, string password);
    }
}
