using ClinicManagementSystem.Models;
using ClinicManagementSystem.Repository;
using ClinicManagementSystem.ViewModel;

namespace ClinicManagementSystem.Service
{
    public class LabTechnicianServiceImpl : ILabTechnicianService
    {
        private readonly ILabTechnicianRepository _repository;

        public LabTechnicianServiceImpl(ILabTechnicianRepository repository)
        {
            _repository = repository;
        }

        public int AddLabTest(LabTestVM model, out string message)
        {
            return _repository.AddLabTest(model, out message);
        }
        public List<LabTechnicianDashboardVM> GetPendingLabTests()
        {
            return _repository.GetPendingLabTests();

        }
        public void AddLabTestResult(LabTestResultVM model)
        {
            _repository.AddLabTestResult(model);
        }
        public LabTestResultVM GetLabTestDetailsForResult(int prescriptionLabTestId)
        {
            return _repository.GetLabTestDetailsForResult(prescriptionLabTestId);
        }
        public List<LabTestResultDisplayVM> GetCompletedLabTests()
        {
            return _repository.GetCompletedLabTests();
        }

        public LabTestResultVM GetLabTestResultById(int id)
        {
            return _repository.GetLabTestResultById(id);
        }

        public void UpdateLabTestResult(LabTestResultVM model)
        {
            _repository.UpdateLabTestResult(model);
        }
       
        public PrescriptionLabBillVM GetPrescriptionLabBill(int prescriptionId)
        {
            return _repository.GetPrescriptionLabBill(prescriptionId);
        }
    }
}