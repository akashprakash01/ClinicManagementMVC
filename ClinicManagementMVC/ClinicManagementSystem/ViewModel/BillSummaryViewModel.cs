namespace ClinicManagementSystem.ViewModel
{
    public class BillSummaryViewModel
    {
        public int TotalItems { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Tax { get; set; }
        public decimal Discount { get; set; }
        public decimal GrandTotal { get; set; }
    }
}
