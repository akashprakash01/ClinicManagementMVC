using ClinicManagementSystem.Models;
using ClinicManagementSystem.ViewModel;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ClinicManagementSystem.Service
{
    public interface IReceptionistService
    {
        public void  InsertPatient(Patient patient, int employeeId);
        IEnumerable<Patient> GetAllPatients();

        public int UpdatePatient(Patient patient, int employeeId);
        public List<Patient> SelectPatients(string contact = null, string name = null);
        public Patient GetPatientById(int patientId);
        public List<AvailableDoctor> SelectAvailableDoctors(int dayOffset);
        public List<AvailableDoctor> GetAllDoctors();
        List<SlotModel> GetDoctorSlots(int doctorId, DateTime selectedDate);

        string InsertAppointment(int doctorId, int patientId,DateTime start, DateTime end);
        public DoctorSlotStatus GetDoctorSlotStatus(int doctorId);
        public List<Appointment> GetDoctorAppointmentsToday(int doctorId);
        public PatientBillViewModel CreatePatientBill(int appointmentId);
        public Appointment GetAppointmentById(int appointmentId);

        public List<AppointmentViewModel> ViewAppointments(
            int? patientId,
            int? doctorId,
            DateTime? appointmentDate,
            DateTime? fromDate,
            DateTime? toDate);

        PatientBillViewModel GetBillById(int billId);

        List<DoctorDropdownVM> GetDoctors();


    }
}
