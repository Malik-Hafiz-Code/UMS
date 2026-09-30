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
    public class clsCourseData
    {
        public static DataTable GetAllCourses()
        {
            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_GetAllCourses", connection))
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

        public static bool GetCourseInfoByCourseID(int courseID, ref int departmentID, ref string name,
            ref int credit, ref int academicLevel)
        {
            bool IsFound = false;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_GetCourseInfoByCourseID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CourseID", courseID);

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                IsFound = true;
                                departmentID = (int)reader["DepartmentID"];
                                name = (string)reader["Name"];
                                credit = (int)reader["Credit"];
                                academicLevel = (int)reader["AcademicLevel"];
                            }
                        }
                    }
                    catch (Exception ex) { MessageBox.Show(ex.ToString()); }
                    finally { connection.Close(); }
                }
            }
            return IsFound;
        }

        public static int AddNewCourse(int departmentID, string name, int credit, int academicLevel)
        {
            int courseID = -1;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_AddNewCourse", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@DepartmentID", departmentID);
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@Credit", credit);
                    command.Parameters.AddWithValue("@AcademicLevel", academicLevel);

                    try
                    {
                        connection.Open();
                        object result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int insertedID))
                            courseID = insertedID;
                    }
                    catch (Exception ex) { MessageBox.Show(ex.ToString()); }
                    finally { connection.Close(); }
                }
            }
            return courseID;
        }

        public static bool UpdateCourse(int courseID, int departmentID, string name, int credit, int academicLevel)
        {
            int rowsAffected = 0;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_UpdateCourse", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@CourseID", courseID);
                    command.Parameters.AddWithValue("@DepartmentID", departmentID);
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@Credit", credit);
                    command.Parameters.AddWithValue("@AcademicLevel", academicLevel);

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

        public static bool DeleteCourse(int courseID)
        {
            int rowsAffected = 0;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_DeleteCourse", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CourseID", courseID);

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

        public static bool IsCourseExist(int courseID)
        {
            bool isFound = false;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_IsCourseExist", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CourseID", courseID);

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
    }
}