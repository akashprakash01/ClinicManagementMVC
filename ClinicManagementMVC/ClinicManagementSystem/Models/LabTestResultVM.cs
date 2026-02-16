namespace ClinicManagementSystem.Models
{
    public class LabTestResultVM
    {
        public int LabTestResultId { get; set; }
        public int PrescriptionLabTestId { get; set; }

        public decimal TotalAmount { get; set; }
        public string Result { get; set; }

    }
}
