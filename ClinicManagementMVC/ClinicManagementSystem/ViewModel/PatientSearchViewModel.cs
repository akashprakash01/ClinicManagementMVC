using ClinicManagementSystem.Models;

namespace ClinicManagementSystem.ViewModel
{
    public class PatientSearchViewModel
    {
        public string SearchContact { get; set; }
        public string SearchName { get; set; }
        public List<Patient> SearchResults { get; set; }
        public Patient NewPatient { get; set; }
    }
}
