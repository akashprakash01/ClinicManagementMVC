namespace ClinicManagementSystem.Models
{
    public class LabTestResultDisplayVM
    {
        public int LabTestResultId { get; set; }
        public string PatientName { get; set; }
        public string LabTestName { get; set; }
        public decimal TotalAmount { get; set; }
        public string Result { get; set; }
        public int PrescriptionId { get; set; }
    }

}
