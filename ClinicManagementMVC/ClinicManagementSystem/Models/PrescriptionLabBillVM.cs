namespace ClinicManagementSystem.Models
{
    public class PrescriptionLabBillVM
    {
        public int PrescriptionId { get; set; }   

        public string PatientName { get; set; }

        public List<LabTestBillItemVM> Tests { get; set; }

        public decimal GrandTotal { get; set; }
    }

    public class LabTestBillItemVM
    {
        public string LabTestName { get; set; }

        public decimal Amount { get; set; }

        public string Result { get; set; }
    }
}
