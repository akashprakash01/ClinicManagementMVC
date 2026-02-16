using Microsoft.AspNetCore.Mvc;

namespace ClinicManagementSystem.Controllers
{
    public class PharmacistController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddMedicine()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddMedicine(string medicineName, string description, decimal price, int quantityStock)
        {
            

            return View();
        }

        public IActionResult ViewMedicines()
        {
            // Fetch medicines from database using your DAL
            return View();
        }
    }
}