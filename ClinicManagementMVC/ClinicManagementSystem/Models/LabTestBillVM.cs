namespace ClinicManagementSystem.Models
{
    public class LabTestBillVM
    {
        public int LabTestResultId { get; set; }

        public string PatientName { get; set; }
        public string LabTestName { get; set; }

        public decimal TotalAmount { get; set; }
        public string Result { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
