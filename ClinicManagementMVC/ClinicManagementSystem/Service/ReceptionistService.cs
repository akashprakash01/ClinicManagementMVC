using ClinicManagementSystem.Models;
using ClinicManagementSystem.Repository;
using ClinicManagementSystem.ViewModel;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ClinicManagementSystem.Service
{
    public class ReceptionistService : IReceptionistService
    {
        private readonly IReceptionistRepository _receptionistRepository;

        public ReceptionistService(IReceptionistRepository receptionistRepository)
        {
            _receptionistRepository = receptionistRepository;
        }

        public void InsertPatient(Patient patient, int employeeId)
        {
             _receptionistRepository.AddPatient(patient,  employeeId); 
        }

        public IEnumerable<Patient> GetAllPatients()
        {
            return _receptionistRepository.GetAllPatients();
        }

        public int UpdatePatient(Patient patient, int employeeId)
        {
            return _receptionistRepository.EditPatient(patient, employeeId);
        }

        public List<Patient> SelectPatients(string contact = null, string name = null)
        {
            return _receptionistRepository.SearchPatients(contact, name);
        }

        public List<AvailableDoctor> SelectAvailableDoctors(int dayOffset)
        {
            return _receptionistRepository.GetAvailableDoctors(dayOffset);
        }
        public string InsertAppointment(int doctorId, int patientId, DateTime start, DateTime end)
        {
            var model = new AppointmentBookingViewModel
            {
                DoctorId = doctorId,
                PatientId = patientId,
                AllocatedTimeDate = start,
                AllocatedTimeUpTo = end
            };

            var result = _receptionistRepository.BookAppointment(model);

            if (result == null)
                return "Booking failed";

            // Return the actual SQL message
            return result.StatusMessage;
        }



        public PatientBillViewModel CreatePatientBill(int appointmentId)
        {
            return _receptionistRepository.GeneratePatientBill(appointmentId);
        }

        public List<AvailableDoctor> GetAllDoctors()
        {
            throw new NotImplementedException();
        }

        public Appointment GetAppointmentById(int id)
        {
            return _receptionistRepository.GetAppointmentById(id);
        }

        public List<Appointment> GetDoctorAppointmentsToday(int doctorId)
        {
            return _receptionistRepository.GetDoctorAppointmentsToday(doctorId);
        }


        public DoctorSlotStatus GetDoctorSlotStatus(int doctorId)
        {
            return _receptionistRepository.GetDoctorSlotStatus(doctorId);
        }


        public Patient GetPatientById(int patientId)
        {
            return _receptionistRepository.GetPatientById(patientId);
        }


        public List<SlotModel> GetDoctorSlots(int doctorId, DateTime selectedDate)
        {
            var slots = _receptionistRepository.GetDoctorSlots(doctorId, selectedDate);

            return slots.Select(s => new SlotModel
            {
                Value = s.StartTime.ToString("yyyy-MM-ddTHH:mm:ss"),
                Text = $"{s.StartTime:hh:mm tt} - {s.EndTime:hh:mm tt}"
            }).ToList();
        }

        public List<AppointmentViewModel> ViewAppointments(
            int? patientId,
            int? doctorId,
            DateTime? appointmentDate,
            DateTime? fromDate,
            DateTime? toDate)
        {
            return _receptionistRepository.ViewAppointments(
                patientId, doctorId, appointmentDate, fromDate, toDate);
        }


        public PatientBillViewModel GetBillById(int billId)
        {
            return _receptionistRepository.GetBillById(billId);
        }

    }
}
