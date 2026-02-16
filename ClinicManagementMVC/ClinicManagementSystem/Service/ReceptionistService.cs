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

        public Appointment GetAppointmentById(int appointmentId)
        {
            throw new NotImplementedException();
        }

        public DashboardViewModel GetDashboardData()
        {
            return _receptionistRepository.GetDashboardData();
        }

        public List<Appointment> GetDoctorAppointmentsToday(int doctorId)
        {
            throw new NotImplementedException();
        }

        public DoctorSlotStatus GetDoctorSlotStatus(int doctorId)
        {
            return _receptionistRepository.GetDoctorSlotStatus(doctorId);
        }


        public Patient GetPatientById(int patientId)
        {
            throw new NotImplementedException();
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


    }
}
