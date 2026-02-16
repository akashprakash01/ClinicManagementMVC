namespace ClinicManagementSystem.Models
{
    public class LabTechnicianDashboardVM
    {
        public int PrescriptionLabTestId { get; set; }

        public string PatientName { get; set; }
        public string LabTestName { get; set; }

        public int PrescriptionId { get; set; }
        public decimal LabTestPrice { get; set; }

        public int Status { get; set; }  // 1 = Pending, 0 = Completed
    }
}
