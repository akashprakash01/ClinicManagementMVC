namespace ClinicManagementSystem.Models
{
    public class DoctorSlotStatus
    {
        public int DoctorId { get; set; }
        public string DoctorName { get; set; }
        public int TotalSlotsPerDay { get; set; }
        public int BookedSlots { get; set; }
        public int RemainingSlots { get; set; }
        public string StatusMessage { get; set; }
        public List<BookedSlot> BookedSlotsList { get; set; }
    }
}
