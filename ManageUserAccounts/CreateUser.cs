using System.Data.SqlClient;

namespace Secretare.ManageUserAccounts
{
    internal class CreateUser
    {
        public static void CreateUserAccount(string username, string password, string firstName, string email, byte[] salt, string lastName = null)
        {
            SqlConnectionStringBuilder csb = new SqlConnectionStringBuilder();
            csb.DataSource = @"(localDB)\MSSQLSERVER02";
            csb.InitialCatalog = "Secretary";
            csb.IntegratedSecurity = true;
            var connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\samuel.wachtel\source\repos\Secretar.io\Secretare\SecretaryDB.mdf;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    using (connection)
                    {
                        string query = "INSERT INTO SecretaryDB.dbo.Users (Username, FirstName, LastName, Email, Password, SaltString) VALUES (@Username, @FirstName, @LastName, @Email, @Password, @SaltString)";
                        using (SqlCommand sqlQuery = new SqlCommand(query))
                        {
                            sqlQuery.Connection = connection;
                            sqlQuery.Parameters.Add("@Username", System.Data.SqlDbType.VarChar, 18).Value = username;
                            sqlQuery.Parameters.Add("@FirstName", System.Data.SqlDbType.VarChar, 40).Value = firstName;
                            sqlQuery.Parameters.Add("@LastName", System.Data.SqlDbType.VarChar, 60).Value = lastName;
                            sqlQuery.Parameters.Add("@Email", System.Data.SqlDbType.VarChar, 100).Value = email;
                            sqlQuery.Parameters.Add("@Password", System.Data.SqlDbType.VarChar, 128).Value = password;
                            sqlQuery.Parameters.Add("@SaltString", System.Data.SqlDbType.VarBinary, 32).Value = salt;
                            connection.Open();
                            sqlQuery.ExecuteNonQuery();
                            connection.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }
    }
}