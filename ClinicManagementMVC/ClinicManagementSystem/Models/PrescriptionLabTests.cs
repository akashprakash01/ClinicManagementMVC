namespace ClinicManagementSystem.Models
{
    public class PrescriptionLabTests
    {
        public int PrescriptionLabTestsId { get; set; }
        public int PrescriptionId { get; set; }
        public int LabTestId { get; set; }
        public  bool Status { get; set; }
        public bool IsDeleted { get; set; }
    }

}
