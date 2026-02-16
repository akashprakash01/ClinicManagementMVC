using ClinicManagementSystem.Models;

namespace ClinicManagementSystem.ViewModel
{
    public class DashboardViewModel
    {
        public int TotalAppointmentsToday { get; set; }
        public int TotalPatientsToday { get; set; }
        public int TotalDoctorsAvailable { get; set; }
        public List<UpcomingAppointment> UpcomingAppointments { get; set; }
    }
}
