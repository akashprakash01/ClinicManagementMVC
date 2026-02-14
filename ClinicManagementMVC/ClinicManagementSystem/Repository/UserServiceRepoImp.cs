using ClassLibraryDatabaseConnection;
using ClinicManagementSystem.Models;
using ClinicManagementSystem.ViewModel;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ClinicManagementSystem.Repository
{
    public class UserServiceRepoImp : IUserRepository
    {
        private readonly string _connectionString;

        public UserServiceRepoImp(string connectionString)
        {
            _connectionString = connectionString;
        }

        public LoginResponse Athentication(string username, string password)
        {
            using (SqlConnection connection = ConnectionManager.OpenConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_login", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", password);

                    // OUTPUT parameters
                    SqlParameter outEmployeeId = new SqlParameter("@outEmployeeId", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(outEmployeeId);

                    SqlParameter outDoctorId = new SqlParameter("@outDoctorId", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(outDoctorId);

                    SqlParameter outRoleId = new SqlParameter("@outRoleId", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(outRoleId);

                    // RETURN parameter
                    SqlParameter returnValue = new SqlParameter();
                    returnValue.Direction = ParameterDirection.ReturnValue;
                    cmd.Parameters.Add(returnValue);

                    cmd.ExecuteNonQuery();

                    LoginResponse response = new LoginResponse();

                    int result = (int)returnValue.Value;

                    if (result == 1)
                    {
                        response.IsSuccess = true;
                        response.EmployeeId = (int?)outEmployeeId.Value;
                        response.RoleId = (int?)outRoleId.Value;

                        if (outDoctorId.Value != DBNull.Value)
                            response.DoctorId = (int?)outDoctorId.Value;
                    }
                    else
                    {
                        response.IsSuccess = false;
                    }

                    return response;
                }
            }
        }

      
    }
}

