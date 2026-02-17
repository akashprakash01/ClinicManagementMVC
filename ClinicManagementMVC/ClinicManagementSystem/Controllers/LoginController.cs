using ClinicManagementSystem.Service;
using ClinicManagementSystem.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagementSystem.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUserServic _userServic;

        public LoginController(IUserServic userServic)
        {
            _userServic = userServic;
        }

        // GET: /Login
        [HttpGet]
        public IActionResult Index()
        {
            return View("Login");
        }

        // POST: /Login/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
                return View("Login", loginViewModel);

            var availableUser = _userServic
                .AuthenticateUserNameAndPassword(
                    loginViewModel.UserName,
                    loginViewModel.Password);

            // ❌ Login failed
            if (!availableUser.IsSuccess)
            {
                ModelState.AddModelError("", "Invalid Username or Password");
                return View("Login", loginViewModel);
            }

            // ✅ Store session values
            HttpContext.Session.SetInt32("EmployeeId", availableUser.EmployeeId ?? 0);
            HttpContext.Session.SetInt32("RoleId", availableUser.RoleId ?? 0);


            if (availableUser.DoctorId.HasValue)
                HttpContext.Session.SetInt32("DoctorId", availableUser.DoctorId.Value);

            // ✅ Role-based redirection
            switch (availableUser.RoleId)
            {
                case 1:
                    return RedirectToAction("Index", "Receptionist");

                case 2:
                    return RedirectToAction("Index", "Doctor");

                case 3:
                    return RedirectToAction("Index", "Pharmacist");

                case 4:
                    return RedirectToAction("Index", "LabTechnician");

                default:
                    ModelState.AddModelError("", "Invalid Role Assigned");
                    return View("Login", loginViewModel);
            }
        }
    }
}