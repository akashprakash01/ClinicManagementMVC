using ClinicManagementSystem.Models;
using ClinicManagementSystem.Repository;
using ClinicManagementSystem.Service;
using ClinicManagementSystem.ViewModel;
using ClinicManagementSystem.ViewModel;

public class UserServiceImp : IUserServic
{
    private readonly IUserRepository _userRepository;

    public UserServiceImp(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public LoginResponse AuthenticateUserNameAndPassword(string username, string password)
    {
        return _userRepository.Athentication(username, password);
    }
}
