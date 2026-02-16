using System.ComponentModel.DataAnnotations;

namespace ClinicManagementSystem.Models
{
    
        public class LabTestVM
        {
            public int LabTestId { get; set; }
            public string LabTestName { get; set; }
            public string Description { get; set; }
        [Range(0, 100000)]
        public decimal LabTestPrice { get; set; }
    }
    }
