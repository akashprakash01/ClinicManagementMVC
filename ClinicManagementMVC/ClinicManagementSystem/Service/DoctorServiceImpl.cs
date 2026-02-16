using ClinicManagementSystem.Models;
using ClinicManagementSystem.Repository;
using ClinicManagementSystem.ViewModel;

namespace ClinicManagementSystem.Service
{
    public class DoctorServiceImpl : IDoctorService
    {
        private readonly IDoctorRepository _doctorRepository;

        public DoctorServiceImpl(IDoctorRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }

        public List<DoctorAppointmentTodayVM> GetDoctorAppointmentsToday(int doctorId)
        {
            return _doctorRepository.GetDoctorAppointmentsToday(doctorId);
        }
        public int AddPrescription(AddPrescriptionVM model)
        {
            return _doctorRepository.AddPrescription(model);
        }

        public List<LabTestVM> GetAvailableLabTests(int prescriptionId)
        {
            return _doctorRepository.GetAvailableLabTests(prescriptionId);
        }

        public void AddPrescriptionLabTest(int prescriptionId, int labTestId)
        {
            _doctorRepository.AddPrescriptionLabTest(prescriptionId, labTestId);
        }
    }
}