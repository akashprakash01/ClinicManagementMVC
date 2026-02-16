namespace ClinicManagementSystem.Models
{
    public class PrescriptionMedicine
    {
        public int PrescriptionMedicineId { get; set; }

        public int PrescriptionId { get; set; }

        public int MedicineId { get; set; }

        public string MedicineName { get; set; }

        public int PrescribedQuantity { get; set; }

        public bool Status { get; set; }
        // 1 = Pending , 0 = Dispensed
    }
}
