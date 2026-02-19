using ClinicManagementSystem.Models;
using ClinicManagementSystem.ViewModel;

namespace ClinicManagementSystem.Service
{
    public interface IDoctorService
    {
        List<DoctorAppointmentTodayVM> GetDoctorAppointmentsToday(int doctorId);
        int AddPrescription(AddPrescriptionVM model);

        List<LabTestVM> GetAvailableLabTests(int prescriptionId);
        void AddPrescriptionLabTest(int prescriptionId, int labTestId);
        public List<MedicineVM> GetAvailableMedicines(int prescriptionId);

        public void AddPrescriptionMedicine(AddPrescriptionMedicineVM model);




    }

}