using ClinicManagementSystem.Models;
using ClinicManagementSystem.ViewModel;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ClinicManagementSystem.Repository
{
    public class DoctorRepositoryImpl : IDoctorRepository
    {
        private readonly string _connectionString;

        public DoctorRepositoryImpl(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<DoctorAppointmentTodayVM> GetDoctorAppointmentsToday(int doctorId)
        {
            List<DoctorAppointmentTodayVM> list = new();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetDoctorAppointmentsToday", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@doctorId", doctorId);

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        list.Add(new DoctorAppointmentTodayVM
                        {
                            AppointmentId = Convert.ToInt32(reader["appointmentId"]),
                            AllocatedAt = Convert.ToDateTime(reader["allocated_at"]),
                            Token = reader["token"].ToString(),
                            PatientId = Convert.ToInt32(reader["patientId"]),
                            PatientName = reader["patientName"].ToString(),
                            Contact = reader["contact"].ToString(),
                            AllocatedTimeDate = Convert.ToDateTime(reader["allocatedTimeDate"]),
                            AllocatedTimeUpTo = Convert.ToDateTime(reader["allocatedTimeUpTo"])
                        });
                    }
                }
            }

            return list;
        }

        public int AddPrescription(AddPrescriptionVM model)
        {
            int prescriptionId = 0;

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_AddPrescription", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@appointmentId", model.AppointmentId);
                    cmd.Parameters.AddWithValue("@doctorId", model.DoctorId);
                    cmd.Parameters.AddWithValue("@symptoms", model.Symptoms);
                    cmd.Parameters.AddWithValue("@diagnosis", model.Diagnosis);
                    cmd.Parameters.AddWithValue("@advice", model.Advice ?? "");
                    cmd.Parameters.AddWithValue("@patientNotes", model.PatientNotes ?? "");
                    cmd.Parameters.AddWithValue("@followUpDate",
                        model.FollowUpDate ?? (object)DBNull.Value);

                    // OUTPUT parameter
                    SqlParameter outId = new SqlParameter("@outPrescriptionId", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(outId);

                    con.Open();
                    cmd.ExecuteNonQuery();

                    // 👇 This line captures the generated ID
                    prescriptionId = Convert.ToInt32(outId.Value);
                }
            }

            return prescriptionId;  // 👈 return the created ID
        }

        public List<LabTestVM> GetAvailableLabTests(int prescriptionId)
        {
            List<LabTestVM> list = new();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_LabTests", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@prescriptionId", prescriptionId);

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        list.Add(new LabTestVM
                        {
                            LabTestId = Convert.ToInt32(reader["labTestId"]),
                            LabTestName = reader["labTestName"].ToString(),
                            Description = reader["description"].ToString()
                        });
                    }
                }
            }

            return list;
        }

        public void AddPrescriptionLabTest(int prescriptionId, int labTestId)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_AddPrescriptionLabTest", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@PrescriptionId", prescriptionId);
                    cmd.Parameters.AddWithValue("@LabTestId", labTestId);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}