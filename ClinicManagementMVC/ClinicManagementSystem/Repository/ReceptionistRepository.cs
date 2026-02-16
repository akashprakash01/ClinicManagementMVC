using ClassLibraryDatabaseConnection;
using ClinicManagementSystem.Models;
using ClinicManagementSystem.ViewModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ClinicManagementSystem.Repository
{
    public class ReceptionistRepository : IReceptionistRepository
    {
        private readonly string _connectionString;
        public ReceptionistRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConnStringMVC");
        }

        #region Add Patient
        public void AddPatient(Patient patient, int employeeId)
        {
            using (SqlConnection conn = ConnectionManager.OpenConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_Addpatient", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@name", patient.Name);
                    cmd.Parameters.AddWithValue("@dob", patient.DOB);
                    cmd.Parameters.AddWithValue("@gender", patient.Gender);
                    cmd.Parameters.AddWithValue("@contact", patient.Contact);
                    cmd.Parameters.AddWithValue("@address", patient.Address);
                    cmd.Parameters.AddWithValue("@employeeId", employeeId);

                    SqlParameter outPatientId = new SqlParameter("@outPatientId", SqlDbType.Int);
                    outPatientId.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(outPatientId);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        #endregion

        #region Update Patient
        public int EditPatient(Patient patient, int employeeId)
        {
            using (SqlConnection conn = ConnectionManager.OpenConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_UpdatePatient", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@patientId", patient.PatientId);
                    cmd.Parameters.AddWithValue("@name", patient.Name);
                    cmd.Parameters.AddWithValue("@dob", patient.DOB);
                    cmd.Parameters.AddWithValue("@gender", patient.Gender);
                    cmd.Parameters.AddWithValue("@contact", patient.Contact);
                    cmd.Parameters.AddWithValue("@address", patient.Address);
                    cmd.Parameters.AddWithValue("@employeeId", employeeId);

                    SqlParameter rowsAffected = new SqlParameter("@rowsAffected", SqlDbType.Int);
                    rowsAffected.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(rowsAffected);
                    cmd.ExecuteNonQuery();
                    return Convert.ToInt32(rowsAffected.Value);
                }
            }
        }
        #endregion


        #region List All Patients
        public IEnumerable<Patient> GetAllPatients()
        {
            List<Patient> patients = new List<Patient>();

            using (SqlConnection conn = ConnectionManager.OpenConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT patientId,name,dob,gender,contact,address,createdAt,updatedAt FROM Patients", conn))
                {
                    cmd.CommandType = CommandType.Text;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            patients.Add(new Patient
                            {
                                PatientId = Convert.ToInt32(reader["patientId"]),
                                Name = reader["name"].ToString(),
                                DOB = Convert.ToDateTime(reader["dob"]),
                                Gender = reader["gender"].ToString(),
                                Contact = reader["contact"].ToString(),
                                Address = reader["address"].ToString(),
                                CreatedAt = reader["createdAt"] as DateTime?,
                                UpdatedAt = reader["updatedAt"] as DateTime?
                            });
                        }
                    }
                }
            }

            return patients;
        }
        #endregion

        #region Search Patients
        public List<Patient> SearchPatients(string contact = null, string name = null)
        {
            List<Patient> patients = new List<Patient>();

            using (SqlConnection conn = ConnectionManager.OpenConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_SearchPatient", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (!string.IsNullOrEmpty(contact))
                        cmd.Parameters.AddWithValue("@contact", contact);
                    else
                        cmd.Parameters.AddWithValue("@contact", DBNull.Value);

                    if (!string.IsNullOrEmpty(name))
                        cmd.Parameters.AddWithValue("@name", name);
                    else
                        cmd.Parameters.AddWithValue("@name", DBNull.Value);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            patients.Add(new Patient
                            {
                                PatientId = Convert.ToInt32(reader["patientId"]),
                                Name = reader["name"].ToString(),
                                DOB = Convert.ToDateTime(reader["dob"]),
                                Gender = reader["gender"].ToString(),
                                Contact = reader["contact"].ToString(),
                                Address = reader["address"].ToString(),
                                CreatedAt = reader["createdAt"] != DBNull.Value ?
                                            Convert.ToDateTime(reader["createdAt"]) : (DateTime?)null,
                                UpdatedAt = reader["updatedAt"] != DBNull.Value ?
                                            Convert.ToDateTime(reader["updatedAt"]) : (DateTime?)null

                            });
                        }
                    }
                }
            }
            return patients;
        }
        #endregion


        public Patient GetPatientById(int patientId)
        {
            throw new NotImplementedException();
        }

        #region Get Available Doctors
        public List<AvailableDoctor> GetAvailableDoctors(int dayOffset)
        {
            List<AvailableDoctor> doctors = new List<AvailableDoctor>();

            using (SqlConnection conn = ConnectionManager.OpenConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_AvailableDoctors", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@dayOffset", dayOffset);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            doctors.Add(new AvailableDoctor
                            {
                                DoctorId = Convert.ToInt32(reader["doctorId"]),
                                DoctorName = reader["doctorName"].ToString(),
                                SpecializationName = reader["specializationName"].ToString(),
                                StartTime = (TimeSpan)reader["startTime"],
                                EndTime = (TimeSpan)reader["endTime"]
                            });
                        }
                    }
                }
            }
            return doctors;
        }
        #endregion

        #region Book Appointment
        public AppointmentBookingViewModel BookAppointment(AppointmentBookingViewModel model)
        {
            using (SqlConnection conn = ConnectionManager.OpenConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_AddAppointment", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@doctorId", model.DoctorId);
                    cmd.Parameters.AddWithValue("@patientId", model.PatientId);
                    cmd.Parameters.AddWithValue("@allocatedTimeDate",model.AllocatedTimeDate);
                    cmd.Parameters.AddWithValue("@allocatedTimeUpTo",model.AllocatedTimeUpTo);


                    SqlParameter outToken = new SqlParameter("@outToken", SqlDbType.VarChar, 12) { Direction = ParameterDirection.Output };
                    SqlParameter outAllocatedTimeDate = new SqlParameter("@outAllocatedTimeDate", SqlDbType.DateTime2) { Direction = ParameterDirection.Output };
                    SqlParameter outAllocatedTimeUpTo = new SqlParameter("@outAllocatedTimeUpTo", SqlDbType.DateTime2) { Direction = ParameterDirection.Output };
                    SqlParameter outAppointmentId = new SqlParameter("@appointmentId", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    SqlParameter outStatusMessage = new SqlParameter("@outStatusMessage", SqlDbType.VarChar, 100) { Direction = ParameterDirection.Output };

                    cmd.Parameters.Add(outToken);
                    cmd.Parameters.Add(outAllocatedTimeDate);
                    cmd.Parameters.Add(outAllocatedTimeUpTo);
                    cmd.Parameters.Add(outAppointmentId);
                    cmd.Parameters.Add(outStatusMessage);

                    cmd.ExecuteNonQuery();

                    model.Token = outToken.Value?.ToString();

                    model.AllocatedTimeDate = outAllocatedTimeDate.Value != DBNull.Value
                        ? Convert.ToDateTime(outAllocatedTimeDate.Value)
                        : DateTime.MinValue;

                    model.AllocatedTimeUpTo = outAllocatedTimeUpTo.Value != DBNull.Value
                        ? Convert.ToDateTime(outAllocatedTimeUpTo.Value)
                        : DateTime.MinValue;

                    model.AppointmentId = outAppointmentId.Value != DBNull.Value
                        ? Convert.ToInt32(outAppointmentId.Value)
                        : 0;

                    model.StatusMessage = outStatusMessage.Value?.ToString();
                    model.IsSuccess = model.AppointmentId > 0;


                    return model;
                }
            }
        }
        #endregion

        #region Patient Bill

        public PatientBillViewModel GeneratePatientBill(int appointmentId)
        {
            PatientBillViewModel model = new PatientBillViewModel();

            using (SqlConnection conn = ConnectionManager.OpenConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GeneratePatientBill", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@appointmentId", appointmentId);

                    SqlParameter outPatientBillId = new SqlParameter("@outPatientBillId", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    SqlParameter outBillAmount = new SqlParameter("@outBillAmount", SqlDbType.Decimal) { Direction = ParameterDirection.Output, Precision = 8, Scale = 2 };
                    SqlParameter outBillStatus = new SqlParameter("@outBillStatus", SqlDbType.VarChar, 10) { Direction = ParameterDirection.Output };
                    SqlParameter outStatusMessage = new SqlParameter("@outStatusMessage", SqlDbType.VarChar, 100) { Direction = ParameterDirection.Output };


                    cmd.Parameters.Add(outPatientBillId);
                    cmd.Parameters.Add(outBillAmount);
                    cmd.Parameters.Add(outBillStatus);
                    cmd.Parameters.Add(outStatusMessage);

                    cmd.ExecuteNonQuery();

                    model.AppointmentId = appointmentId;
                    model.PatientBillId = outPatientBillId.Value != DBNull.Value ? Convert.ToInt32(outPatientBillId.Value) : 0;
                    model.BillAmount = outBillAmount.Value != DBNull.Value ? Convert.ToDecimal(outBillAmount.Value) : 0;
                    model.BillStatus = outBillStatus.Value?.ToString();
                    model.StatusMessage = outStatusMessage.Value?.ToString();
                    model.IsSuccess = model.PatientBillId > 0;
                }
            }
            return model;
        }
        #endregion
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
            DashboardViewModel dashboard = new DashboardViewModel();
            dashboard.UpcomingAppointments = new List<UpcomingAppointment>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                // Total appointments today
                string queryAppointments = @"SELECT COUNT(*) FROM Appointment 
                                           WHERE CAST(allocatedTimeDate AS DATE) = CAST(GETDATE() AS DATE)";
                using (SqlCommand cmd = new SqlCommand(queryAppointments, conn))
                {
                    dashboard.TotalAppointmentsToday = (int)cmd.ExecuteScalar();
                }

                // Total patients registered today
                string queryPatients = @"SELECT COUNT(*) FROM Patients 
                                       WHERE CAST(createdAt AS DATE) = CAST(GETDATE() AS DATE)";
                using (SqlCommand cmd = new SqlCommand(queryPatients, conn))
                {
                    dashboard.TotalPatientsToday = (int)cmd.ExecuteScalar();
                }

                // Available doctors today
                string queryDoctors = @"SELECT COUNT(*) FROM DoctorsWorkingHours dwh
                                      INNER JOIN WeekDays w ON dwh.dayId = w.dayId
                                      WHERE w.dayNames = DATENAME(WEEKDAY, GETDATE())
                                      AND dwh.isAvailable = 1";
                using (SqlCommand cmd = new SqlCommand(queryDoctors, conn))
                {
                    dashboard.TotalDoctorsAvailable = (int)cmd.ExecuteScalar();
                }

                // Upcoming appointments (next 3)
                string queryUpcoming = @"SELECT TOP 3 a.appointmentId, a.token, a.allocatedTimeDate,
                                        p.name AS patientName, CONCAT(e.firstName, ' ', e.lastName) AS doctorName
                                        FROM Appointment a
                                        INNER JOIN Patients p ON a.patientId = p.patientId
                                        INNER JOIN Doctors d ON a.doctorId = d.doctorId
                                        INNER JOIN EmployeeProfile e ON d.employeeId = e.employeeId
                                        WHERE CAST(a.allocatedTimeDate AS DATE) = CAST(GETDATE() AS DATE)
                                        AND a.allocatedTimeDate >= GETDATE()
                                        ORDER BY a.allocatedTimeDate";

                using (SqlCommand cmd = new SqlCommand(queryUpcoming, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            dashboard.UpcomingAppointments.Add(new UpcomingAppointment
                            {
                                AppointmentId = Convert.ToInt32(reader["appointmentId"]),
                                PatientName = reader["patientName"].ToString(),
                                DoctorName = reader["doctorName"].ToString(),
                                Time = Convert.ToDateTime(reader["allocatedTimeDate"]),
                                Token = reader["token"].ToString()
                            });
                        }
                    }
                }
            }

            return dashboard;
        }

        public List<Appointment> GetDoctorAppointmentsToday(int doctorId)
        {
            throw new NotImplementedException();
        }

        public List<DoctorSlot> GetDoctorSlots(int doctorId, DateTime selectedDate)
        {
            var slots = new List<DoctorSlot>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_GetDoctorSlots", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@DoctorId", doctorId);
                    cmd.Parameters.AddWithValue("@SelectedDate", selectedDate);

                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            slots.Add(new DoctorSlot
                            {
                                StartTime = reader.GetDateTime(0),
                                EndTime = reader.GetDateTime(1)
                            });
                        }
                    }
                }
            }

            return slots;
        }



        public DoctorSlotStatus GetDoctorSlotStatus(int doctorId)
        {
            DoctorSlotStatus status = new DoctorSlotStatus();

            using (SqlConnection conn = ConnectionManager.OpenConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_DoctorSlotStatusToday", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@doctorId", SqlDbType.Int).Value = doctorId;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        // ✅ RESULT SET 1 (SUMMARY)
                        if (reader.Read())
                        {
                            status.DoctorId = Convert.ToInt32(reader["doctorId"]);
                            status.DoctorName = reader["doctorName"].ToString();
                            status.TotalSlotsPerDay = Convert.ToInt32(reader["totalSlotsPerDay"]);
                            status.BookedSlots = Convert.ToInt32(reader["bookedSlots"]);
                            status.RemainingSlots = Convert.ToInt32(reader["remainingSlots"]);
                            status.StatusMessage = reader["statusMessage"].ToString();
                        }

                        // ✅ Move to Result Set 2
                        if (reader.NextResult())
                        {
                            while (reader.Read())
                            {
                                status.BookedSlotsList.Add(new BookedSlot
                                {
                                    SlotStartTime = Convert.ToDateTime(reader["slotStartTime"]),
                                    SlotEndTime = Convert.ToDateTime(reader["slotEndTime"])
                                });
                            }
                        }
                    }
                }
            }

            return status;
        }

        public List<DoctorWorkingHours> GetDoctorWorkingHours(int doctorId, int dayId)
        {
            var workingHours = new List<DoctorWorkingHours>();

            using (SqlConnection conn = ConnectionManager.OpenConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetDoctorWorkingHours", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@DoctorId", doctorId);
                    cmd.Parameters.AddWithValue("@DayId", dayId);

                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        workingHours.Add(new DoctorWorkingHours
                        {
                            StartTime = (TimeSpan)dr["StartTime"],
                            EndTime = (TimeSpan)dr["EndTime"]
                        });
                    }
                }
            }

            return workingHours;
        }

        public List<BookedSlot> GetBookedSlots(int doctorId, DateTime selectedDate)
        {
            var bookedSlots = new List<BookedSlot>();

            using (SqlConnection conn = ConnectionManager.OpenConnection(_connectionString))
            {
                string query = @"SELECT allocatedTimeDate as SlotStartTime, 
                                allocatedTimeUpTo as SlotEndTime
                         FROM Appointment
                         WHERE doctorId = @doctorId
                         AND CAST(allocatedTimeDate AS DATE) = @selectedDate";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@doctorId", doctorId);
                    cmd.Parameters.AddWithValue("@selectedDate", selectedDate.Date);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            bookedSlots.Add(new BookedSlot
                            {
                                SlotStartTime = Convert.ToDateTime(reader["SlotStartTime"]),
                                SlotEndTime = Convert.ToDateTime(reader["SlotEndTime"])
                            });
                        }
                    }
                }
            }

            return bookedSlots;
        }

    }
}
