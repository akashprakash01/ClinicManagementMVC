using ClassLibraryDatabaseConnection;
using ClinicManagementSystem.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ClinicManagementSystem.Repository
{
    public class PharmacistRepository : IPharmacistRepository
    {
        // Field
        private readonly string _connectionString;

        // Dependency Injection
        public PharmacistRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConnStringMVC");
        }
        public void AddBillItem(int billId, int prescriptionMedicineId, int quantity)
        {
            using (SqlConnection conn = ConnectionManager.OpenConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_AddPharmacyBillItem", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@PharmacyBillId", billId);
                cmd.Parameters.AddWithValue("@PrescriptionMedicineId", prescriptionMedicineId);
                cmd.Parameters.AddWithValue("@Quantity", quantity);

                cmd.ExecuteNonQuery();
            }
        }

        public void AddMedicine(Medicine model)
        {
            using (SqlConnection conn = ConnectionManager.OpenConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_AddMedicine", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@medicineName", model.MedicineName);
                cmd.Parameters.AddWithValue("@description", model.Description);
                cmd.Parameters.AddWithValue("@medicinetypeid", model.MedicineTypeId);
                cmd.Parameters.AddWithValue("@price", model.Price);
                cmd.Parameters.AddWithValue("@quantitystock", model.QuantityStock);
                cmd.Parameters.AddWithValue("@CreatedBy", model.CreatedBy);

                cmd.ExecuteNonQuery();
            }
        }

        public void AddMedicineType(MedicineType model)
        {
            using (SqlConnection conn = ConnectionManager.OpenConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_AddMedicineType", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@TypeName", model.TypeName);
                    cmd.Parameters.AddWithValue("@CreatedBy", model.CreatedBy);

                    
                    cmd.ExecuteNonQuery();
                }
                    
            }
        }

        public int CreatePharmacyBill(int prescriptionId, int createdBy)
        {
            using (SqlConnection conn = ConnectionManager.OpenConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_CreatePharmacyBill", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@PrescriptionId", prescriptionId);
                cmd.Parameters.AddWithValue("@CreatedBy", createdBy);

                SqlParameter output = new SqlParameter("@OutPharmacyBillId", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };

                cmd.Parameters.Add(output);

                cmd.ExecuteNonQuery();

                return Convert.ToInt32(output.Value);
            }
        }

        public PharmacyBill GetBillByPrescription(int prescriptionId)
        {
            PharmacyBill bill = null;

            using (SqlConnection conn = ConnectionManager.OpenConnection(_connectionString))
            {
                string query = "SELECT * FROM PharmacyBill WHERE PrescriptionId=@id";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", prescriptionId);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    bill = new PharmacyBill
                    {
                        PharmacyBillId = Convert.ToInt32(dr["PharmacyBillId"]),
                        TotalAmount = Convert.ToDecimal(dr["TotalAmount"]),
                        PaymentStatus = Convert.ToBoolean(dr["PaymentStatus"])
                    };
                }
            }
            return bill;
        }

        public List<PharmacyBillItem> GetBillItems(int billId)
        {
            List<PharmacyBillItem> list = new List<PharmacyBillItem>();

            using (SqlConnection conn = ConnectionManager.OpenConnection(_connectionString))
            {
                string query = @"SELECT pbi.*, m.MedicineName,
                        (pbi.Quantity * pbi.UnitPrice) AS SubTotal
                        FROM PharmacyBillItems pbi
                        INNER JOIN PrescriptionMedicines pm 
                        ON pbi.PrescriptionMedicineId = pm.PrescriptionMedicineId
                        INNER JOIN Medicines m 
                        ON pm.MedicineId = m.MedicineId
                        WHERE PharmacyBillId = @id";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", billId);

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    list.Add(new PharmacyBillItem
                    {
                        MedicineName = dr["MedicineName"].ToString(),
                        Quantity = Convert.ToInt32(dr["Quantity"]),
                        UnitPrice = Convert.ToDecimal(dr["UnitPrice"]),
                        SubTotal = Convert.ToDecimal(dr["SubTotal"])
                    });
                }
            }
            return list;
        }

        public List<Medicine> GetMedicines()
        {
            List<Medicine> list = new List<Medicine>();

            using (SqlConnection conn = ConnectionManager.OpenConnection(_connectionString))
            {
                string query = @"SELECT m.*, mt.TypeName 
                         FROM Medicines m
                         INNER JOIN MedicineType mt
                         ON m.MedicineTypeId = mt.MedicineTypeId";

                SqlCommand cmd = new SqlCommand(query, conn);

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    list.Add(new Medicine
                    {
                        MedicineId = Convert.ToInt32(dr["MedicineId"]),
                        MedicineName = dr["MedicineName"].ToString(),
                        Description = dr["Description"].ToString(),
                        Price = Convert.ToDecimal(dr["Price"]),
                        QuantityStock = Convert.ToInt32(dr["QuantityStock"]),
                        MedicineTypeName = dr["TypeName"].ToString()
                    });
                }
            }
            return list;
        }

        public List<MedicineType> GetMedicineTypes()
        {
            List<MedicineType> list = new List<MedicineType>();

            using (SqlConnection conn = ConnectionManager.OpenConnection(_connectionString))
            {
                string query = "SELECT MedicineTypeId, TypeName FROM MedicineType";

                SqlCommand cmd = new SqlCommand(query, conn);

               
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    list.Add(new MedicineType
                    {
                        MedicineTypeId = Convert.ToInt32(dr["MedicineTypeId"]),
                        TypeName = dr["TypeName"].ToString()
                    });
                }
            }
            return list;
        }

        public List<PrescriptionMedicine> GetPendingPrescriptionMedicines(int prescriptionId)
        {
            List<PrescriptionMedicine> list = new List<PrescriptionMedicine>();

            using (SqlConnection conn = ConnectionManager.OpenConnection(_connectionString))
            {
                string query = @"SELECT pm.PrescriptionMedicineId, 
                                m.MedicineName,
                                pm.Quantity,
                                pm.Status
                         FROM PrescriptionMedicines pm
                         INNER JOIN Medicines m 
                         ON pm.MedicineId = m.MedicineId
                         WHERE pm.PrescriptionId = @id
                         AND pm.Status = 1";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", prescriptionId);

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    list.Add(new PrescriptionMedicine
                    {
                        PrescriptionMedicineId = Convert.ToInt32(dr["PrescriptionMedicineId"]),
                        MedicineName = dr["MedicineName"].ToString(),
                        PrescribedQuantity = Convert.ToInt32(dr["Quantity"]),
                        Status = Convert.ToBoolean(dr["Status"])
                    });
                }
            }
            return list;
        }

        public void PayBill(int billId)
        {
            using (SqlConnection conn = ConnectionManager.OpenConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_PayPharmacyBill", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@PharmacyBillId", billId);

                cmd.ExecuteNonQuery();
            }
        }
    }
}
