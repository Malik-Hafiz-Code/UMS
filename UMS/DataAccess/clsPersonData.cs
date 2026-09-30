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
    public class clsPersonData
    {
        public static DataTable GetAllPeople()
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = "SP_GetAllPeople";//2 exec SP_GetAllPeole and without cmd.CommandType
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.CommandType= CommandType.StoredProcedure;
                    try
                    {
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                                dt.Load(reader);
                        }
                    }
                    catch (Exception ex) { MessageBox.Show(ex.ToString()); }
                    finally { conn.Close(); }
                }
            }
            return dt;
        }
        public static bool GetPersonInfoByPersonID(int personID, ref string name, ref string phone,
            ref string email, ref DateTime dateOfBirth, ref char gender, ref string address)
        {
            bool IsFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "exec SP_GetPersonInfoByPersonID @personID";
            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@personID", personID);
            try
            {
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    IsFound=true;
                    name=(string)reader["Name"];
                    phone=(string)reader["Phone"];
                    email=(string)reader["Email"];
                    dateOfBirth=(DateTime)reader["DateOfBirth"];
                    gender=Convert.ToChar(reader["Gender"]);
                    address=(string)reader["Address"];
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally { connection.Close(); }
            return IsFound;
        }
        public static int AddNewPerson(string name, string phone, string email,
               DateTime dateOfBirth, char gender, string address)
        {
            int personID = -1;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "SP_AddNewPerson";
            SqlCommand command = new SqlCommand(query, connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@phone", phone);
            command.Parameters.AddWithValue("@email", email);
            command.Parameters.AddWithValue("@dateofbirth", dateOfBirth);
            command.Parameters.AddWithValue("@gender", gender);
            command.Parameters.AddWithValue("@address", address);

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                    personID = insertedID;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                connection.Close();
            }
            return personID;
        }
        public static bool UpdatePerson(int personID, string name, string phone, string email,
            DateTime dateOfBirth, char gender, string address)
        {
            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "SP_UpdatePerson";
            SqlCommand command = new SqlCommand(query, connection);
            command.CommandType= CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@personID", personID);
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@phone", phone);
            command.Parameters.AddWithValue("@email", email);
            command.Parameters.AddWithValue("@dateofbirth", dateOfBirth);
            command.Parameters.AddWithValue("@gender", gender);
            command.Parameters.AddWithValue("@address", address);
            try
            {
                connection.Open();
                rowsAffected=command.ExecuteNonQuery();
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
            finally { connection.Close(); }
            return rowsAffected>0;
        }
        public static bool DeletePerson(int personID)
        {
            int rowsAffected = 0;
            SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "SP_DeletePerson";
            SqlCommand command = new SqlCommand(query, conn);
            command.CommandType= CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@personID", personID);
            try
            {
                conn.Open();
                rowsAffected= command.ExecuteNonQuery();
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
            finally { conn.Close(); }
            return rowsAffected>0;
        }
        public static bool IsPersonExist(int personID)
        {
            bool isFound=false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "SP_IsPersonExist";
            SqlCommand command = new SqlCommand(query, connection);
            command.CommandType=CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@personID", personID);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if(reader.HasRows)
                    isFound=true;
                reader.Close();
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
            finally { connection.Close(); }
            return isFound;
        }
        public static int CountPeople()
        {
            int count = 0;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_GetCountPeople", connection))
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
    }
}
