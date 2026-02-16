using ClinicManagementSystem.Models;

namespace ClinicManagementSystem.Repository
{
    public interface ILabTechnicianRepository
    {
        int AddLabTest(LabTestVM model, out string message);
        List<LabTechnicianDashboardVM> GetPendingLabTests();
        void AddLabTestResult(LabTestResultVM model);

        LabTestResultVM GetLabTestDetailsForResult(int prescriptionLabTestId);
        List<LabTestResultDisplayVM> GetCompletedLabTests();

        LabTestResultVM GetLabTestResultById(int id);
        void UpdateLabTestResult(LabTestResultVM model);
        PrescriptionLabBillVM GetPrescriptionLabBill(int prescriptionId);
    }

}
