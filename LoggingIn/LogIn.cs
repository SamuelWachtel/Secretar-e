using Secretare.Features;
using System.Data.SqlClient;
namespace Secretare.LoggingIn
{
    internal class LogIn
    {
        public static int LoggedInUserId { get; set; }
        public void Login()
        { 
            Console.WriteLine("LOG IN");
            Console.WriteLine("Enter your username:");
            string usernameInput = Console.ReadLine();
            Console.WriteLine("Enter your password:");
            string passwordInput = Console.ReadLine();
            string userIdString = ValidateUser(usernameInput, passwordInput);
            if (int.TryParse(userIdString, out int userId))
            {
                LoggedInUserId = userId;
                Console.WriteLine("Login successful");
            }
            else
            {
                Console.WriteLine("Login failed");
            }
        }
        public string ValidateUser(string usernameInput, string passwordInput)
        {
            List<Account> Usernames = new List<Account>();
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
                        string query = "SELECT UserId, Username, Password, SaltString FROM SecretaryDB.dbo.Users WHERE Username = @Username";
                        using (SqlCommand sqlQuery = new SqlCommand(query, connection))
                        {
                            sqlQuery.Parameters.Add("@Username", System.Data.SqlDbType.VarChar, 18).Value = usernameInput;
                            connection.Open();
                            using (SqlDataReader reader = sqlQuery.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    var storedPassword = reader["Password"].ToString();
                                    byte[] storedSalt = (byte[])reader["SaltString"];
                                    var storedUserId = reader["UserId"].ToString();
                                    var hashedInputPassword = PasswordHasher.HashPassword(passwordInput, storedSalt);

                                    if (storedPassword == hashedInputPassword)
                                    {
                                        return storedUserId;
                                    }
                                }
                            }   
                        }
                    }
                }
                catch (Exception ex)
                {
                    return "Login failed";
                }
                return "Login failed";
            }
        }

        public class Account
        {
            public string Username { get; set; }
            public string Password { get; set; }
            public byte[] Salt { get; set; }
            public string UserId { get; set; }

            public Account(string username, string password, byte[] salt, string userId)
            {
                Username = username;
                Password = password;
                Salt = salt;
                UserId = userId;
            }
        }
    }
}
