using ClinicManagementSystem.Models;

namespace ClinicManagementSystem.ViewModel
{
    public class DispenseViewModel
    {
        public int PrescriptionId { get; set; }
        public string PatientName { get; set; }
        public string DoctorName { get; set; }
        public List<PrescriptionMedicine> PrescribedMedicines { get; set; }
        public List<Medicine> AvailableMedicines { get; set; }
    }
}
