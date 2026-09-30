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
    public class clsSectionData
    {
        public static DataTable GetAllSections()
        {
            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_GetAllSections", connection))
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

        public static bool GetSectionInfoBySectionID(int sectionID, ref int courseID, ref int doctorID,
            ref int year, ref string semester, ref int room)
        {
            bool IsFound = false;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_GetSectionInfoBySectionID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@SectionID", sectionID);

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                IsFound = true;
                                courseID = (int)reader["CourseID"];
                                doctorID = (int)reader["DoctorID"];
                                year = (int)reader["Year"];
                                semester = (string)reader["Semester"];
                                room = (int)reader["Room"];
                            }
                        }
                    }
                    catch (Exception ex) { MessageBox.Show(ex.ToString()); }
                    finally { connection.Close(); }
                }
            }
            return IsFound;
        }

        public static int AddNewSection(int courseID, int doctorID, int year, string semester, int room)
        {
            int sectionID = -1;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_AddNewSection", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@CourseID", courseID);
                    command.Parameters.AddWithValue("@DoctorID", doctorID);
                    command.Parameters.AddWithValue("@Year", year);
                    command.Parameters.AddWithValue("@Semester", semester);
                    command.Parameters.AddWithValue("@Room", room);

                    try
                    {
                        connection.Open();
                        object result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int insertedID))
                            sectionID = insertedID;
                    }
                    catch (Exception ex) { MessageBox.Show(ex.ToString()); }
                    finally { connection.Close(); }
                }
            }
            return sectionID;
        }

        public static bool UpdateSection(int sectionID, int courseID, int doctorID, int year,
            string semester, int room)
        {
            int rowsAffected = 0;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_UpdateSection", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@SectionID", sectionID);
                    command.Parameters.AddWithValue("@CourseID", courseID);
                    command.Parameters.AddWithValue("@DoctorID", doctorID);
                    command.Parameters.AddWithValue("@Year", year);
                    command.Parameters.AddWithValue("@Semester", semester);
                    command.Parameters.AddWithValue("@Room", room);

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

        public static bool DeleteSection(int sectionID)
        {
            int rowsAffected = 0;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_DeleteSection", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@SectionID", sectionID);

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

        public static bool IsSectionExist(int sectionID)
        {
            bool isFound = false;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_IsSectionExist", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
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

        public static DataTable GetAllSectionsRaw()
        {
            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_GetAllSectionsRaw", connection))
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
    }
}