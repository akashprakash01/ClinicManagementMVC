using ClinicManagementSystem.Models;

namespace ClinicManagementSystem.Service
{
    public interface ILabTechnicianService
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
