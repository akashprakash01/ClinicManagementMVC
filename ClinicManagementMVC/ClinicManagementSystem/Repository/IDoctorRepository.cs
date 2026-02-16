using ClinicManagementSystem.Models;
using ClinicManagementSystem.ViewModel;

namespace ClinicManagementSystem.Repository
{
    public interface IDoctorRepository
    {
        List<DoctorAppointmentTodayVM> GetDoctorAppointmentsToday(int doctorId);
        int AddPrescription(AddPrescriptionVM model);

        public List<LabTestVM> GetAvailableLabTests(int prescriptionId);

        void AddPrescriptionLabTest(int prescriptionId, int labTestId);

    }
}
