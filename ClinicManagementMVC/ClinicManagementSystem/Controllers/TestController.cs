using ClassLibraryDatabaseConnection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using NuGet.Protocol.Plugins;

namespace ClinicManagementSystem.Controllers
{
    public class TestController : Controller
    {

        //field
        public readonly IConfiguration _configuration;

        //Dependency injection
        public TestController(IConfiguration configuration )
        {
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            string connectionString = _configuration.GetConnectionString("ConnStringMVC");

            //test connection
            try
            {
                using (SqlConnection connection = ConnectionManager.OpenConnection(connectionString))
                {
                    if (connection != null)
                    {
                        SqlCommand command = new SqlCommand("Select DB_NAME() as DatabaseName," +
                        " @@SERVERNAME AS ServerName FROM INFORMATION_SCHEMA.TABLES", connection);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                ViewBag.Message = "Connection Successful";
                            }
                        }
                    }


                }
                //using (SqlConnection connection = new SqlConnection(connectionString))
                //{
                //    connection.Open();
                //    SqlCommand command = new SqlCommand("Select DB_NAME() as DatabaseName," +
                //        " @@SERVERNAME AS ServerName FROM INFORMATION_SCHEMA.TABLES", connection);
                //    using (SqlDataReader reader = command.ExecuteReader())
                //    {
                //        if (reader.Read())
                //        {
                //            ViewBag.Message = "Connection Successful";
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Connection Failed";
                ViewBag.Exception = ex;
            }
            return View();
        }
    }
}
