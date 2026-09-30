using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DataAccess
{
    public class clsUserData
    {
        public static DataTable GetAllUsers()
        {
            DataTable dt = new DataTable();

            SqlConnection connection =
                new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "SP_GetAllUsers";

            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                connection.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                    dt.Load(reader);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                connection.Close();
            }

            return dt;
        }

        public static bool GetUserInfoByUserID(
            int UserID,
            ref int PersonID,
            ref string Username,
            ref string Password,
            ref string Role,
            ref bool IsActive)
        {
            bool IsFound = false;

            using (SqlConnection connection =
                new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command =
                    new SqlCommand("SP_GetUserInfoByUserID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@UserID", UserID);

                    try
                    {
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                IsFound = true;

                                PersonID = (int)reader["PersonID"];
                                Username = (string)reader["Username"];
                                Password = (string)reader["Password"];
                                Role = (string)reader["Role"];
                                IsActive = (bool)reader["IsActive"];
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }
            }

            return IsFound;
        }

        public static bool GetUserInfoByPersonID(
            ref int UserID,
            int PersonID,
            ref string Username,
            ref string Password,
            ref string Role,
            ref bool IsActive)
        {
            bool IsFound = false;

            using (SqlConnection connection =
                new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command =
                    new SqlCommand("SP_GetUserInfoByPersonID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@PersonID", PersonID);

                    try
                    {
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                IsFound = true;

                                UserID = (int)reader["UserID"];
                                Username = (string)reader["Username"];
                                Password = (string)reader["Password"];
                                Role = (string)reader["Role"];
                                IsActive = (bool)reader["IsActive"];
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }
            }

            return IsFound;
        }

        public static bool FindByUsernameAndPassword(
            string username,
            string password,
            ref int userID,
            ref int personID,
            ref string role,
            ref bool isActive)
        {
            bool isFound = false;

            using (SqlConnection connection =
                new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command =
                    new SqlCommand("SP_FindByUsernameAndPassword", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", password);

                    try
                    {
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                isFound = true;

                                userID = (int)reader["UserID"];
                                personID = (int)reader["PersonID"];
                                role = (string)reader["Role"];
                                isActive = (bool)reader["IsActive"];
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }
            }

            return isFound;
        }

        public static int AddNewUser(
            int PersonID,
            string Username,
            string Password,
            string Role,
            bool IsActive)
        {
            int UserID = -1;

            using (SqlConnection connection =
                new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command =
                    new SqlCommand("SP_AddNewUser", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@PersonID", PersonID);
                    command.Parameters.AddWithValue("@Username", Username);
                    command.Parameters.AddWithValue("@Password", Password);
                    command.Parameters.AddWithValue("@Role", Role);
                    command.Parameters.AddWithValue("@IsActive", IsActive);

                    try
                    {
                        connection.Open();

                        object result = command.ExecuteScalar();

                        if (result != null &&
                            int.TryParse(result.ToString(), out int InsertedID))
                        {
                            UserID = InsertedID;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }
            }

            return UserID;
        }

        public static bool UpdateUser(
            int UserID,
            int PersonID,
            string Username,
            string Password,
            string Role,
            bool IsActive)
        {
            int RowsAffected = 0;

            using (SqlConnection connection =
                new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand cmd =
                    new SqlCommand("SP_UpdateUser", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@UserID", UserID);
                    cmd.Parameters.AddWithValue("@PersonID", PersonID);
                    cmd.Parameters.AddWithValue("@Username", Username);
                    cmd.Parameters.AddWithValue("@Password", Password);
                    cmd.Parameters.AddWithValue("@Role", Role);
                    cmd.Parameters.AddWithValue("@IsActive", IsActive);

                    try
                    {
                        connection.Open();

                        RowsAffected = cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }
            }

            return RowsAffected > 0;
        }

        public static bool DeleteUser(int UserID)
        {
            int RowsAffected = 0;

            using (SqlConnection connection =
                new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand cmd =
                    new SqlCommand("SP_DeleteUser", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@UserID", UserID);

                    try
                    {
                        connection.Open();

                        RowsAffected = cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }
            }

            return RowsAffected > 0;
        }

        public static bool IsUserExist(int UserID)
        {
            bool IsFound = false;

            using (SqlConnection connection =
                new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand cmd =
                    new SqlCommand("SP_IsUserExistByUserID", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@UserID", UserID);

                    try
                    {
                        connection.Open();

                        SqlDataReader reader = cmd.ExecuteReader();

                        IsFound = reader.HasRows;

                        reader.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }
            }

            return IsFound;
        }

        public static bool IsUserExist(string Username)
        {
            bool IsFound = false;

            using (SqlConnection connection =
                new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand cmd =
                    new SqlCommand("SP_IsUserExistByUsername", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Username", Username);

                    try
                    {
                        connection.Open();

                        SqlDataReader reader = cmd.ExecuteReader();

                        IsFound = reader.HasRows;

                        reader.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }
            }

            return IsFound;
        }

        public static bool IsUserExistByPersonID(int PersonID)
        {
            bool IsFound = false;

            using (SqlConnection connection =
                new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand cmd =
                    new SqlCommand("SP_IsUserExistByPersonID", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@PersonID", PersonID);

                    try
                    {
                        connection.Open();

                        SqlDataReader reader = cmd.ExecuteReader();

                        IsFound = reader.HasRows;

                        reader.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }
            }

            return IsFound;
        }

        public static bool ChangePassword(int UserID, string NewPassword)
        {
            int rowsAffected = 0;

            using (SqlConnection connection =
                new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command =
                    new SqlCommand("SP_ChangePassword", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@UserID", UserID);
                    command.Parameters.AddWithValue("@Password", NewPassword);

                    try
                    {
                        connection.Open();

                        rowsAffected = command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }
            }

            return rowsAffected > 0;
        }

        public static int CountAllUsers()
        {
            int count = 0;

            using (SqlConnection connection =
                new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command =
                    new SqlCommand("SP_GetCountUsers", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    SqlParameter outputParam =
                        new SqlParameter("@count", SqlDbType.Int);

                    outputParam.Direction = ParameterDirection.Output;

                    command.Parameters.Add(outputParam);

                    try
                    {
                        connection.Open();

                        command.ExecuteNonQuery();

                        if (outputParam.Value != DBNull.Value)
                            count = (int)outputParam.Value;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }

            return count;
        }

        public static int CountActiveUsers()
        {
            int count = 0;

            using (SqlConnection connection =
                new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command =
                    new SqlCommand("SP_GetCountActiveUsers", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    SqlParameter outputParam =
                        new SqlParameter("@count", SqlDbType.Int);

                    outputParam.Direction = ParameterDirection.Output;

                    command.Parameters.Add(outputParam);

                    try
                    {
                        connection.Open();

                        command.ExecuteNonQuery();

                        if (outputParam.Value != DBNull.Value)
                            count = (int)outputParam.Value;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }

            return count;
        } 
    }
}