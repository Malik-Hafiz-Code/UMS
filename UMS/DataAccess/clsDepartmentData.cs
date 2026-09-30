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
    public class clsDepartmentData
    {
        public static DataTable GetAllDepartments()
        {
            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_GetAllDepartments", connection))
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

        public static bool GetDepartmentInfoByDepartmentID(int departmentID, ref string name, ref string building)
        {
            bool IsFound = false;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_GetDepartmentInfoByID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@DepartmentID", departmentID);

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                IsFound = true;
                                name = (string)reader["Name"];
                                building = (string)reader["Building"];
                            }
                        }
                    }
                    catch (Exception ex) { MessageBox.Show(ex.ToString()); }
                    finally { connection.Close(); }
                }
            }
            return IsFound;
        }

        public static int AddNewDepartment(string name, string building)
        {
            int departmentID = -1;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_AddNewDepartment", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@Building", building);

                    try
                    {
                        connection.Open();
                        object result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int insertedID))
                            departmentID = insertedID;
                    }
                    catch (Exception ex) { MessageBox.Show(ex.ToString()); }
                    finally { connection.Close(); }
                }
            }
            return departmentID;
        }

        public static bool UpdateDepartment(int departmentID, string name, string building)
        {
            int rowsAffected = 0;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_UpdateDepartment", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@DepartmentID", departmentID);
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@Building", building);

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

        public static bool DeleteDepartment(int departmentID)
        {
            int rowsAffected = 0;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_DeleteDepartment", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@DepartmentID", departmentID);

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

        public static bool IsDepartmentExist(int departmentID)
        {
            bool isFound = false;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_IsDepartmentExist", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@DepartmentID", departmentID);

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                                isFound = true;
                        }
                    }
                    catch (Exception ex) { MessageBox.Show(ex.ToString()); }
                    finally { connection.Close(); }
                }
            }
            return isFound;
        }
        public static int CountDepartments()
        {
            int count = 0;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_GetCountDepartments", connection))
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
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                    finally { connection.Close(); }
                }
            }
                return count;
        }
    }
}