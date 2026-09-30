using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataAccess
{
    public class clsDoctorData
    {
        public static DataTable GetAllDoctors()
        {
            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_GetAllDoctors", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                                dt.Load(reader);
                        }
                    }
                    catch (Exception ex) { MessageBox.Show(ex.ToString()); }
                    finally { connection.Close(); }
                }
            }
            return dt;
        }

        public static bool GetDoctorInfoByDoctorID(int doctorID, ref int personID, ref int departmentID,
            ref decimal salary, ref bool isActive)
        {
            bool IsFound = false;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_GetDoctorInfoByDoctorID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@DoctorID", doctorID);

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                IsFound = true;
                                personID = (int)reader["PersonID"];
                                departmentID = (int)reader["DepartmentID"];
                                salary = Convert.ToDecimal(reader["Salary"]);
                                isActive = (bool)reader["Status"];
                            }
                        }
                    }
                    catch (Exception ex) { MessageBox.Show(ex.ToString()); }
                    finally { connection.Close(); }
                }
            }
            return IsFound;
        }

        public static int AddNewDoctor(int personID, int departmentID, decimal salary, bool isActive)
        {
            int doctorID = -1;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_AddNewDoctor", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@PersonID", personID);
                    command.Parameters.AddWithValue("@DepartmentID", departmentID);
                    command.Parameters.AddWithValue("@Salary", salary);
                    command.Parameters.AddWithValue("@Status", isActive);

                    try
                    {
                        connection.Open();
                        object result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int insertedID))
                            doctorID = insertedID;
                    }
                    catch (Exception ex) { MessageBox.Show(ex.ToString()); }
                    finally { connection.Close(); }
                }
            }
            return doctorID;
        }

        public static bool UpdateDoctor(int doctorID, int personID, int departmentID, decimal salary,bool isActive)
        {
            int rowsAffected = 0;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_UpdateDoctor", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@DoctorID", doctorID);
                    command.Parameters.AddWithValue("@PersonID", personID);
                    command.Parameters.AddWithValue("@DepartmentID", departmentID);
                    command.Parameters.AddWithValue("@Salary", salary);
                    command.Parameters.AddWithValue("@Status", isActive);

                    try
                    {
                        connection.Open();
                        rowsAffected = command.ExecuteNonQuery();
                    }
                    catch (Exception ex) { MessageBox.Show(ex.ToString()); }
                    finally { connection.Close(); }
                }
            }
            return rowsAffected > 0;
        }

        public static bool DeleteDoctor(int doctorID)
        {
            int rowsAffected = 0;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_DeleteDoctor", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@DoctorID", doctorID);

                    try
                    {
                        connection.Open();
                        rowsAffected = command.ExecuteNonQuery();
                    }
                    catch (Exception ex) { MessageBox.Show(ex.ToString()); }
                    finally { connection.Close(); }
                }
            }
            return rowsAffected > 0;
        }

        public static bool IsDoctorExist(int doctorID)
        {
            bool isFound = false;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_IsDoctorExist", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@DoctorID", doctorID);

                    try
                    {
                        connection.Open();
                        object result = command.ExecuteScalar();
                        isFound = (result != null && Convert.ToInt32(result) == 1);
                    }
                    catch (Exception ex) { MessageBox.Show(ex.ToString()); }
                    finally { connection.Close(); }
                }
            }
            return isFound;
        }

        public static bool IsPersonDoctor(int personID)
        {
            bool isFound = false;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_IsPersonDoctor", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@PersonID", personID);

                    try
                    {
                        connection.Open();
                        object result = command.ExecuteScalar();
                        isFound = (result != null && Convert.ToInt32(result) == 1);
                    }
                    catch (Exception ex) { MessageBox.Show(ex.ToString()); }
                    finally { connection.Close(); }
                }
            }
            return isFound;
        }

        public static int CountDoctors()
        {
            int count = 0;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_GetCountDoctors", connection))
                {
                    command.CommandType= CommandType.StoredProcedure;
                    SqlParameter outputParam = new SqlParameter("@count", SqlDbType.Int);
                    outputParam.Direction = ParameterDirection.Output;
                    command.Parameters.Add(outputParam);
                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        if (outputParam.Value!=DBNull.Value)
                            count=(int)outputParam.Value;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                    finally { connection.Close(); }
                }
            }
            return count;
        }
        public static bool IsDoctorByPersonID(int PersonID)
        {
            bool IsFound = false;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = "SP_IsDoctorByPersonID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@PersonID", PersonID);

                    try
                    {
                        connection.Open();

                        object result = command.ExecuteScalar();

                        if (result != null && Convert.ToInt32(result) > 0)
                            IsFound = true;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }
            }

            return IsFound;
        }
    }
}