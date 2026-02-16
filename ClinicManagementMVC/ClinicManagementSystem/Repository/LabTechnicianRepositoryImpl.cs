using ClinicManagementSystem.Models;
using ClinicManagementSystem.ViewModel;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ClinicManagementSystem.Repository
{
    public class LabTechnicianRepositoryImpl : ILabTechnicianRepository
    {
        private readonly string _connectionString;

        public LabTechnicianRepositoryImpl(string connectionString)
        {
            _connectionString = connectionString;
        }

        public int AddLabTest(LabTestVM model, out string message)
        {
            int labTestId = 0;
            message = "";

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_AddLabTest", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@labTestName", model.LabTestName);
                    cmd.Parameters.AddWithValue("@description", model.Description ?? "");
                    cmd.Parameters.AddWithValue("@LabTestPrice", model.LabTestPrice);

                    SqlParameter outId = new SqlParameter("@outLabTestId", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(outId);

                    SqlParameter outMsg = new SqlParameter("@outStatusMessage", SqlDbType.VarChar, 100)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(outMsg);

                    con.Open();
                    cmd.ExecuteNonQuery();

                    labTestId = outId.Value != DBNull.Value ? Convert.ToInt32(outId.Value) : 0;
                    message = outMsg.Value?.ToString();
                }
            }

            return labTestId;
        }
        public List<LabTechnicianDashboardVM> GetPendingLabTests()
        {
            List<LabTechnicianDashboardVM> list = new();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetPendingLabTests", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        list.Add(new LabTechnicianDashboardVM
                        {
                            PrescriptionLabTestId = Convert.ToInt32(reader["prescriptionLabTestId"]),
                            PrescriptionId = Convert.ToInt32(reader["prescriptionId"]),
                            PatientName = reader["PatientName"].ToString(),
                            LabTestName = reader["labTestName"].ToString(),
                            LabTestPrice = Convert.ToDecimal(reader["LabTestPrice"]),
                            Status = Convert.ToInt32(reader["status"])
                        });
                    }
                }
            }

            return list;
        }
        public void AddLabTestResult(LabTestResultVM model)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_AddLabTestResult", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@PrescriptionLabTestId", model.PrescriptionLabTestId);
                    cmd.Parameters.AddWithValue("@Result", model.Result ?? "");

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public LabTestResultVM GetLabTestDetailsForResult(int prescriptionLabTestId)
        {
            LabTestResultVM model = new();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                string query = @"
            SELECT lt.LabTestPrice
            FROM PrescriptionLabTests plt
            INNER JOIN LabTests lt
                ON lt.labTestId = plt.labTestId
            WHERE plt.prescriptionLabTestId = @Id";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Id", prescriptionLabTestId);

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        model.PrescriptionLabTestId = prescriptionLabTestId;
                        model.TotalAmount = Convert.ToDecimal(reader["LabTestPrice"]);
                    }
                }
            }

            return model;
        }

        public List<LabTestResultDisplayVM> GetCompletedLabTests()
        {
            List<LabTestResultDisplayVM> list = new();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetCompletedLabTests", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        list.Add(new LabTestResultDisplayVM
                        {
                            LabTestResultId = Convert.ToInt32(reader["LabTestResultId"]),
                            PrescriptionId = Convert.ToInt32(reader["prescriptionId"]), // 🔥 CRITICAL FIX
                            PatientName = reader["PatientName"].ToString(),
                            LabTestName = reader["labTestName"].ToString(),
                            TotalAmount = Convert.ToDecimal(reader["TotalAmount"]),
                            Result = reader["Result"].ToString()
                        });
                    }
                }
            }

            return list;
        }

        public LabTestResultVM GetLabTestResultById(int id)
        {
            LabTestResultVM model = new();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                string query = "SELECT LabTestResultId, Result FROM LabTestResult WHERE LabTestResultId = @Id";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    con.Open();

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        model.LabTestResultId = Convert.ToInt32(reader["LabTestResultId"]);
                        model.Result = reader["Result"].ToString();
                    }
                }
            }

            return model;
        }

        public void UpdateLabTestResult(LabTestResultVM model)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_UpdateLabTestResult", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@LabTestResultId", model.LabTestResultId);
                    cmd.Parameters.AddWithValue("@UpdatedResult", model.Result ?? "");

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public LabTestBillVM GetLabTestBillById(int id)
        {
            LabTestBillVM model = new();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetLabTestBillById", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@LabTestResultId", id);

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        model.LabTestResultId = Convert.ToInt32(reader["LabTestResultId"]);
                        model.PatientName = reader["PatientName"].ToString();
                        model.LabTestName = reader["labTestName"].ToString();
                        model.TotalAmount = Convert.ToDecimal(reader["TotalAmount"]);
                        model.Result = reader["Result"].ToString();
                        model.CreatedDate = Convert.ToDateTime(reader["CreatedDate"]);
                    }
                }
            }

            return model;
        }

        public PrescriptionLabBillVM GetPrescriptionLabBill(int prescriptionId)
        {
            PrescriptionLabBillVM model = new();
            model.Tests = new List<LabTestBillItemVM>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetPrescriptionLabBill", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PrescriptionId", prescriptionId);

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        if (model.PatientName == null)
                            model.PatientName = reader["PatientName"].ToString();

                        decimal amount = Convert.ToDecimal(reader["TotalAmount"]);

                        model.Tests.Add(new LabTestBillItemVM
                        {
                            LabTestName = reader["labTestName"].ToString(),
                            Amount = amount,
                            Result = reader["Result"].ToString()
                        });

                        model.GrandTotal += amount;
                    }
                }
            }

            return model;
        }
    }
}