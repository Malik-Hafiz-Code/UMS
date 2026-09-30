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
    public class clsEnrollmentData
    {
        public static DataTable GetAllEnrollments()
        {
            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_GetAllEnrollments", connection))
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

        public static bool GetEnrollmentInfoByEnrollmentID(int enrollmentID, ref int studentID, ref int sectionID,
            ref double grade, ref string status, ref DateTime enrollmentDate)
        {
            bool IsFound = false;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_GetEnrollmentInfoByEnrollmentID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@EnrollmentID", enrollmentID);

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                IsFound = true;
                                studentID = (int)reader["StudentID"];
                                sectionID = (int)reader["SectionID"];
                                grade = (double)reader["Grade"];
                                status = (string)reader["Status"];
                                enrollmentDate = (DateTime)reader["EnrollmentDate"];
                            }
                        }
                    }
                    catch (Exception ex) { MessageBox.Show(ex.ToString()); }
                    finally { connection.Close(); }
                }
            }
            return IsFound;
        }

        public static int AddNewEnrollment(int studentID, int sectionID, double grade, string status,
            DateTime enrollmentDate)
        {
            int enrollmentID = -1;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_AddNewEnrollment", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@StudentID", studentID);
                    command.Parameters.AddWithValue("@SectionID", sectionID);
                    command.Parameters.AddWithValue("@Grade", grade);
                    command.Parameters.AddWithValue("@Status", status);
                    command.Parameters.AddWithValue("@EnrollmentDate", enrollmentDate);

                    try
                    {
                        connection.Open();
                        object result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int insertedID))
                            enrollmentID = insertedID;
                    }
                    catch (Exception ex) { MessageBox.Show(ex.ToString()); }
                    finally { connection.Close(); }
                }
            }
            return enrollmentID;
        }

        public static bool UpdateEnrollment(int enrollmentID, int studentID, int sectionID, double grade,
            string status, DateTime enrollmentDate)
        {
            int rowsAffected = 0;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_UpdateEnrollment", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@EnrollmentID", enrollmentID);
                    command.Parameters.AddWithValue("@StudentID", studentID);
                    command.Parameters.AddWithValue("@SectionID", sectionID);
                    command.Parameters.AddWithValue("@Grade", grade);
                    command.Parameters.AddWithValue("@Status", status);
                    command.Parameters.AddWithValue("@EnrollmentDate", enrollmentDate);

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

        public static bool DeleteEnrollment(int enrollmentID)
        {
            int rowsAffected = 0;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_DeleteEnrollment", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@EnrollmentID", enrollmentID);

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

        public static bool IsEnrollmentExist(int enrollmentID)
        {
            bool isFound = false;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_IsEnrollmentExist", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@EnrollmentID", enrollmentID);

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

        public static bool IsStudentEnrolledInSection(int studentID, int sectionID)
        {
            bool isFound = false;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_IsStudentEnrolledInSection", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@StudentID", studentID);
                    command.Parameters.AddWithValue("@SectionID", sectionID);

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