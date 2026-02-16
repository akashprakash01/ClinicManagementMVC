using ClinicManagementSystem.Models;
using ClinicManagementSystem.ViewModel;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ClinicManagementSystem.Repository
{
    public interface IReceptionistRepository
    {
        public void AddPatient(Patient patient, int employeeId);
        IEnumerable<Patient> GetAllPatients();
        public int EditPatient(Patient patient, int employeeId);
        public List<Patient> SearchPatients( string contact = null, string name = null);
        public Patient GetPatientById(int patientId);
        public List<AvailableDoctor> GetAvailableDoctors(int dayOffset);
        public List<AvailableDoctor> GetAllDoctors();
        public List<DoctorWorkingHours> GetDoctorWorkingHours(int doctorId, int dayId);
        public List<BookedSlot> GetBookedSlots(int doctorId, DateTime selectedDate);
        public AppointmentBookingViewModel BookAppointment(AppointmentBookingViewModel model);
        public DoctorSlotStatus GetDoctorSlotStatus(int doctorId);

        public List<Appointment> GetDoctorAppointmentsToday(int doctorId);
        public PatientBillViewModel GeneratePatientBill(int appointmentId);
        public Appointment GetAppointmentById(int appointmentId);
        public DashboardViewModel GetDashboardData();

        List<DoctorSlot> GetDoctorSlots(int doctorId, DateTime selectedDate);

    }
}
