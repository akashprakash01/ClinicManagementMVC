using ClassLibraryDatabaseConnection;
using ClinicManagementSystem.Models;
using ClinicManagementSystem.ViewModel;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace ClinicManagementSystem.Repository
{
    public class PharmacistRepository : IPharmacistRepository
    {
        private readonly string _connectionString;

        public PharmacistRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConnStringMVC");
        }

        // ================= PRESCRIPTION METHODS =================

        public IEnumerable<Prescription> GetPendingPrescriptions()
        {
            List<Prescription> prescriptions = new List<Prescription>();

            using (SqlConnection con = ConnectionManager.OpenConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand(@"
                SELECT DISTINCT 
                    p.prescriptionId,
                    p.createdAt AS prescriptionDate,
                    pt.name AS PatientName,
                    CONCAT(ep.firstName, ' ', ep.lastName) AS DoctorName

                FROM Prescription p

                INNER JOIN Appointment a 
                    ON p.appointmentId = a.appointmentId

                INNER JOIN Patients pt 
                    ON a.patientId = pt.patientId

                INNER JOIN Doctors d
                    ON p.doctorId = d.doctorId

                INNER JOIN EmployeeProfile ep
                    ON d.employeeId = ep.employeeId

                WHERE EXISTS (
                    SELECT 1
                    FROM PrescriptionMedicines pm
                    WHERE pm.prescriptionId = p.prescriptionId
                    AND pm.Status = 1
                )

                ORDER BY p.createdAt DESC", con);


                cmd.CommandType = CommandType.Text;

                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Prescription prescription = new Prescription
                    {
                        PrescriptionId = Convert.ToInt32(rdr["prescriptionId"]),
                        PrescriptionDate = Convert.ToDateTime(rdr["prescriptionDate"]),
                        PatientName = rdr["PatientName"].ToString(),
                        DoctorName = rdr["DoctorName"].ToString()
                    };

                    prescriptions.Add(prescription);
                }
            }

            return prescriptions;
        }


        public Prescription GetPrescriptionById(int id)
        {
            Prescription prescription = null;

            using (SqlConnection con = ConnectionManager.OpenConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand(@"
            SELECT 
                p.prescriptionId,
                p.createdAt AS prescriptionDate,
                pt.name AS PatientName,
                CONCAT(ep.firstName,' ',ep.lastName) AS DoctorName
            FROM Prescription p
            INNER JOIN Appointment a ON p.appointmentId = a.appointmentId
            INNER JOIN Patients pt ON a.patientId = pt.patientId
            INNER JOIN Doctors d ON p.doctorId = d.doctorId
            INNER JOIN EmployeeProfile ep ON d.employeeId = ep.employeeId
            WHERE p.prescriptionId = @PrescriptionId", con);

                cmd.Parameters.AddWithValue("@PrescriptionId", id);

                SqlDataReader rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    prescription = new Prescription
                    {
                        PrescriptionId = Convert.ToInt32(rdr["prescriptionId"]),
                        PrescriptionDate = Convert.ToDateTime(rdr["prescriptionDate"]),
                        PatientName = rdr["PatientName"].ToString(),
                        DoctorName = rdr["DoctorName"].ToString()
                    };
                }
            }

            return prescription;
        }


        public int GetPendingMedicineCount(int prescriptionId)
        {
            using (SqlConnection con = ConnectionManager.OpenConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand(@"
                    SELECT COUNT(*) 
                    FROM PrescriptionMedicines 
                    WHERE prescriptionId = @PrescriptionId AND status = 1", con);

                cmd.Parameters.AddWithValue("@PrescriptionId", prescriptionId);

                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        public List<PrescriptionMedicine> GetPrescriptionMedicines(int prescriptionId)
        {
            List<PrescriptionMedicine> medicines = new List<PrescriptionMedicine>();

            using (SqlConnection con = ConnectionManager.OpenConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand(@"
            SELECT pm.*, m.MedicineName, m.Price
            FROM PrescriptionMedicines pm
            INNER JOIN Medicines m ON pm.medicineId = m.MedicineId
            WHERE pm.prescriptionId = @PrescriptionId
            ORDER BY pm.status DESC, pm.PrescriptionMedicineId", con);

                cmd.Parameters.AddWithValue("@PrescriptionId", prescriptionId);

                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    medicines.Add(new PrescriptionMedicine
                    {
                        PrescriptionMedicineId = Convert.ToInt32(rdr["PrescriptionMedicineId"]),
                        PrescriptionId = Convert.ToInt32(rdr["prescriptionId"]),
                        MedicineId = Convert.ToInt32(rdr["medicineId"]),
                        MedicineName = rdr["MedicineName"].ToString(),

                        Dosage = rdr["dosage"]?.ToString(),
                        Frequency = rdr["frequency"]?.ToString(),
                        Duration = rdr["duration"]?.ToString(),

                        // ✅ REAL QUANTITY
                        Quantity = rdr["quantity"] != DBNull.Value
                            ? Convert.ToInt32(rdr["quantity"])
                            : 0,

                        Price = rdr["Price"] != DBNull.Value
                            ? Convert.ToDecimal(rdr["Price"])
                            : 0,

                        Status = rdr["status"] != DBNull.Value
                            ? Convert.ToInt32(rdr["status"])
                            : 0
                    });
                }
            }

            return medicines;
        }


        public IEnumerable<Medicine> GetAvailableMedicinesForPrescription(int prescriptionId)
        {
            List<Medicine> medicines = new List<Medicine>();

            using (SqlConnection con = ConnectionManager.OpenConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_Medicines", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@prescriptionId", prescriptionId);

                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Medicine medicine = new Medicine
                    {
                        MedicineId = Convert.ToInt32(rdr["medicineId"]),
                        MedicineName = rdr["medicineName"].ToString(),
                        Description = rdr["description"] != DBNull.Value ? rdr["description"].ToString() : ""
                    };
                    medicines.Add(medicine);
                }
            }

            return medicines;
        }

        // ================= MEDICINE METHODS =================

        public IEnumerable<Medicine> GetAllMedicines()
        {
            List<Medicine> medicines = new List<Medicine>();

            using (SqlConnection con = ConnectionManager.OpenConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand(@"
                    SELECT m.*, mt.TypeName 
                    FROM Medicines m
                    INNER JOIN MedicineType mt ON m.MedicineTypeId = mt.MedicineTypeId
                    ORDER BY m.MedicineId DESC", con);
                cmd.CommandType = CommandType.Text;

                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Medicine medicine = new Medicine
                    {
                        MedicineId = Convert.ToInt32(rdr["MedicineId"]),
                        MedicineName = rdr["MedicineName"].ToString(),
                        Description = rdr["Description"] != DBNull.Value ? rdr["Description"].ToString() : "",
                        MedicineTypeId = Convert.ToInt32(rdr["MedicineTypeId"]),
                        MedicineTypeName = rdr["TypeName"].ToString(),
                        Price = Convert.ToDecimal(rdr["Price"]),
                        QuantityStock = Convert.ToInt32(rdr["QuantityStock"]),
                        CreatedBy = Convert.ToInt32(rdr["createdBy"]),
                        CreatedAt = Convert.ToDateTime(rdr["createdAt"])
                    };
                    medicines.Add(medicine);
                }
            }

            return medicines;
        }

        public Medicine GetMedicineById(int id)
        {
            Medicine medicine = null;

            using (SqlConnection con = ConnectionManager.OpenConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Medicines WHERE MedicineId = @MedicineId", con);
                cmd.Parameters.AddWithValue("@MedicineId", id);

                SqlDataReader rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    medicine = new Medicine
                    {
                        MedicineId = Convert.ToInt32(rdr["MedicineId"]),
                        MedicineName = rdr["MedicineName"].ToString(),
                        Description = rdr["Description"] != DBNull.Value ? rdr["Description"].ToString() : "",
                        MedicineTypeId = Convert.ToInt32(rdr["MedicineTypeId"]),
                        Price = Convert.ToDecimal(rdr["Price"]),
                        QuantityStock = Convert.ToInt32(rdr["QuantityStock"]),
                        CreatedBy = Convert.ToInt32(rdr["createdBy"]),
                        CreatedAt = Convert.ToDateTime(rdr["createdAt"])
                    };
                }
            }

            return medicine;
        }

        public int AddMedicine(Medicine medicine)
        {
            using (SqlConnection con = ConnectionManager.OpenConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_AddMedicine", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@medicineName", medicine.MedicineName);
                cmd.Parameters.AddWithValue("@description", medicine.Description ?? "");
                cmd.Parameters.AddWithValue("@medicinetypeid", medicine.MedicineTypeId);
                cmd.Parameters.AddWithValue("@price", medicine.Price);
                cmd.Parameters.AddWithValue("@quantitystock", medicine.QuantityStock);
                cmd.Parameters.AddWithValue("@CreatedBy", medicine.CreatedBy);

                return cmd.ExecuteNonQuery();
            }
        }

        public bool UpdateMedicine(Medicine medicine)
        {
            using (SqlConnection con = ConnectionManager.OpenConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand(@"
                    UPDATE Medicines 
                    SET MedicineName = @MedicineName,
                        Description = @Description,
                        MedicineTypeId = @MedicineTypeId,
                        Price = @Price,
                        QuantityStock = @QuantityStock
                    WHERE MedicineId = @MedicineId", con);

                cmd.Parameters.AddWithValue("@MedicineId", medicine.MedicineId);
                cmd.Parameters.AddWithValue("@MedicineName", medicine.MedicineName);
                cmd.Parameters.AddWithValue("@Description", medicine.Description ?? "");
                cmd.Parameters.AddWithValue("@MedicineTypeId", medicine.MedicineTypeId);
                cmd.Parameters.AddWithValue("@Price", medicine.Price);
                cmd.Parameters.AddWithValue("@QuantityStock", medicine.QuantityStock);

                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        public bool DeleteMedicine(int id)
        {
            using (SqlConnection con = ConnectionManager.OpenConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM Medicines WHERE MedicineId = @MedicineId", con);
                cmd.Parameters.AddWithValue("@MedicineId", id);

                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        // ================= MEDICINE TYPE METHODS =================

        public IEnumerable<MedicineType> GetAllMedicineTypes()
        {
            List<MedicineType> types = new List<MedicineType>();

            using (SqlConnection con = ConnectionManager.OpenConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM MedicineType ORDER BY TypeName", con);
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    MedicineType type = new MedicineType
                    {
                        MedicineTypeId = Convert.ToInt32(rdr["MedicineTypeId"]),
                        TypeName = rdr["TypeName"].ToString(),
                        CreatedBy = Convert.ToInt32(rdr["createdBy"]),
                        CreatedAt = Convert.ToDateTime(rdr["createdAt"])
                    };
                    types.Add(type);
                }
            }

            return types;
        }

        public int AddMedicineType(MedicineType medicineType)
        {
            using (SqlConnection con = ConnectionManager.OpenConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_AddMedicineType", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@TypeName", medicineType.TypeName);
                cmd.Parameters.AddWithValue("@CreatedBy", medicineType.CreatedBy);

                return cmd.ExecuteNonQuery();
            }
        }

        // ================= BILL METHODS =================

        public PharmacyBill GetPharmacyBill(int prescriptionId)
        {
            PharmacyBill bill = null;

            using (SqlConnection con = ConnectionManager.OpenConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand(@"
        SELECT 
            pb.*,
            pt.name AS PatientName,
            CONCAT(ep.firstName, ' ', ep.lastName) AS DoctorName,
            p.createdAt AS prescriptionDate

        FROM PharmacyBill pb

        INNER JOIN Prescription p 
            ON pb.PrescriptionId = p.prescriptionId

        INNER JOIN Appointment a 
            ON p.appointmentId = a.appointmentId

        INNER JOIN Patients pt 
            ON a.patientId = pt.patientId

        INNER JOIN Doctors d
            ON p.doctorId = d.doctorId

        INNER JOIN EmployeeProfile ep 
            ON d.employeeId = ep.employeeId

        WHERE pb.PrescriptionId = @PrescriptionId", con);

                cmd.Parameters.AddWithValue("@PrescriptionId", prescriptionId);

                SqlDataReader rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    bill = new PharmacyBill
                    {
                        PharmacyBillId = Convert.ToInt32(rdr["PharmacyBillId"]),
                        PrescriptionId = Convert.ToInt32(rdr["PrescriptionId"]),
                        TotalAmount = Convert.ToDecimal(rdr["TotalAmount"]),
                        BillDate = Convert.ToDateTime(rdr["BillDate"]),
                        PaymentStatus = Convert.ToInt32(rdr["PaymentStatus"]),
                        PatientName = rdr["PatientName"].ToString(),
                        DoctorName = rdr["DoctorName"].ToString(),
                        PrescriptionDate = Convert.ToDateTime(rdr["prescriptionDate"])
                    };
                }
            }

            return bill;
        }


        public PharmacyBill GetPharmacyBillById(int billId)
        {
            PharmacyBill bill = null;

            using (SqlConnection con = ConnectionManager.OpenConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand(@"
        SELECT 
            pb.*,
            pt.name AS PatientName,
            CONCAT(ep.firstName, ' ', ep.lastName) AS DoctorName,
            p.createdAt AS prescriptionDate

        FROM PharmacyBill pb

        INNER JOIN Prescription p 
            ON pb.PrescriptionId = p.prescriptionId

        INNER JOIN Appointment a 
            ON p.appointmentId = a.appointmentId

        INNER JOIN Patients pt 
            ON a.patientId = pt.patientId

        INNER JOIN Doctors d
            ON p.doctorId = d.doctorId

        INNER JOIN EmployeeProfile ep 
            ON d.employeeId = ep.employeeId

        WHERE pb.PharmacyBillId = @BillId", con);

                cmd.Parameters.AddWithValue("@BillId", billId);

                SqlDataReader rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    bill = new PharmacyBill
                    {
                        PharmacyBillId = Convert.ToInt32(rdr["PharmacyBillId"]),
                        PrescriptionId = Convert.ToInt32(rdr["PrescriptionId"]),
                        TotalAmount = Convert.ToDecimal(rdr["TotalAmount"]),
                        BillDate = Convert.ToDateTime(rdr["BillDate"]),
                        PaymentStatus = Convert.ToInt32(rdr["PaymentStatus"]),
                        PatientName = rdr["PatientName"].ToString(),
                        DoctorName = rdr["DoctorName"].ToString(),
                        PrescriptionDate = Convert.ToDateTime(rdr["prescriptionDate"])
                    };
                }
            }

            return bill;
        }


        public int CreatePharmacyBill(int prescriptionId, int createdBy)
        {
            using (SqlConnection con = ConnectionManager.OpenConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_CreatePharmacyBill", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@PrescriptionId", prescriptionId);
                cmd.Parameters.AddWithValue("@CreatedBy", createdBy);

                SqlParameter outputParam = new SqlParameter("@OutPharmacyBillId", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(outputParam);

                cmd.ExecuteNonQuery();

                return Convert.ToInt32(outputParam.Value);
            }
        }

        public bool AddPharmacyBillItem(int pharmacyBillId, int prescriptionMedicineId, int quantity)
        {
            using (SqlConnection con = ConnectionManager.OpenConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_AddPharmacyBillItem", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@PharmacyBillId", pharmacyBillId);
                cmd.Parameters.AddWithValue("@PrescriptionMedicineId", prescriptionMedicineId);
                cmd.Parameters.AddWithValue("@Quantity", quantity);

                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        public bool RemovePharmacyBillItem(int billItemId)
        {
            using (SqlConnection con = ConnectionManager.OpenConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand(@"
                    DELETE FROM PharmacyBillItems 
                    WHERE PharmacyBillItemId = @BillItemId", con);

                cmd.Parameters.AddWithValue("@BillItemId", billItemId);

                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        public List<PharmacyBillItem> GetPharmacyBillItems(int pharmacyBillId)
        {
            List<PharmacyBillItem> items = new List<PharmacyBillItem>();

            using (SqlConnection con = ConnectionManager.OpenConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand(@"
                    SELECT 
                        pbi.PharmacyBillItemId,
                        pbi.PharmacyBillId,
                        pbi.PrescriptionMedicineId,
                        pbi.Quantity,
                        pbi.UnitPrice,
                        pbi.SubTotal,
                        ISNULL(m.MedicineName,'') AS MedicineName,
                        ISNULL(pm.dosage,'') AS dosage,
                        ISNULL(pm.frequency,'') AS frequency
                    FROM PharmacyBillItems pbi
                    LEFT JOIN PrescriptionMedicines pm 
                        ON pbi.PrescriptionMedicineId = pm.PrescriptionMedicineId
                    LEFT JOIN Medicines m 
                        ON pm.MedicineId = m.MedicineId
                    WHERE pbi.PharmacyBillId = @PharmacyBillId", con);

                cmd.Parameters.AddWithValue("@PharmacyBillId", pharmacyBillId);

                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    PharmacyBillItem item = new PharmacyBillItem
                    {
                        PharmacyBillItemId = Convert.ToInt32(rdr["PharmacyBillItemId"]),
                        PharmacyBillId = Convert.ToInt32(rdr["PharmacyBillId"]),
                        PrescriptionMedicineId = Convert.ToInt32(rdr["PrescriptionMedicineId"]),
                        Quantity = Convert.ToInt32(rdr["Quantity"]),
                        UnitPrice = Convert.ToDecimal(rdr["UnitPrice"]),
                        MedicineName = rdr["MedicineName"].ToString(),
                        Dosage = rdr["dosage"].ToString(),
                        Frequency = rdr["frequency"].ToString(),
                    };
                    items.Add(item);
                }
            }

            return items;
        }

        public bool PayPharmacyBill(int pharmacyBillId)
        {
            using (SqlConnection con = ConnectionManager.OpenConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_PayPharmacyBill", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PharmacyBillId", pharmacyBillId);

                cmd.ExecuteNonQuery();
                return true;
            }
        }

        public bool CheckStock(int prescriptionMedicineId, int quantity)
        {
            using SqlConnection con = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand(@"
                SELECT m.QuantityStock
                FROM Medicines m
                INNER JOIN PrescriptionMedicines pm 
                    ON pm.MedicineId = m.MedicineId
                WHERE pm.PrescriptionMedicineId = @PrescriptionMedicineId", con);

            cmd.Parameters.AddWithValue("@PrescriptionMedicineId", prescriptionMedicineId);

            con.Open();

            var stockObj = cmd.ExecuteScalar();

            if (stockObj == null)
                return false;

            int stock = Convert.ToInt32(stockObj);

            return stock >= quantity;
        }

        public bool UpdateDispenseStatus(int prescriptionMedicineId, int quantity)
        {
            using SqlConnection con = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand(@"
                UPDATE PrescriptionMedicines
                SET Status = 0
                WHERE PrescriptionMedicineId = @PrescriptionMedicineId", con);

            cmd.Parameters.AddWithValue("@PrescriptionMedicineId", prescriptionMedicineId);

            con.Open();

            return cmd.ExecuteNonQuery() > 0;
        }

        public bool ReduceStock(int prescriptionMedicineId, int quantity)
        {
            using SqlConnection con = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand(@"
                UPDATE Medicines
                SET QuantityStock = QuantityStock - @Qty
                WHERE MedicineId = (
                SELECT MedicineId 
                FROM PrescriptionMedicines 
                WHERE PrescriptionMedicineId = @PrescriptionMedicineId)", con);

            cmd.Parameters.AddWithValue("@Qty", quantity);
            cmd.Parameters.AddWithValue("@PrescriptionMedicineId", prescriptionMedicineId);

            con.Open();

            return cmd.ExecuteNonQuery() > 0;
        }

        public bool AddDispensedMedicinesToBill(int prescriptionId, int billId)
        {
            using SqlConnection con = ConnectionManager.OpenConnection(_connectionString);

            SqlCommand cmd = new SqlCommand(@"
        INSERT INTO PharmacyBillItems
        (PharmacyBillId, PrescriptionMedicineId, Quantity, UnitPrice)

        SELECT 
            @BillId,
            pm.PrescriptionMedicineId,
            pm.Quantity,
            m.Price

        FROM PrescriptionMedicines pm
        INNER JOIN Medicines m ON pm.MedicineId = m.MedicineId
        WHERE pm.PrescriptionId = @PrescriptionId
        AND pm.Status = 0   -- ONLY DISPENSED
    ", con);

            cmd.Parameters.AddWithValue("@BillId", billId);
            cmd.Parameters.AddWithValue("@PrescriptionId", prescriptionId);

            return cmd.ExecuteNonQuery() > 0;
        }



    }
}