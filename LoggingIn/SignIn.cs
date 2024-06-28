using Secretare.Features;
using Secretare.ManageUserAccounts;
using System.Data.SqlClient;
using static Secretare.LoggingIn.LogIn;

namespace Secretare.LoggingIn
{
    internal class SignIn
    {

        public void SignInNewUser()
        {
            var allUsernames = new List<string> { };

            //get all usernames from the database and pass them to allUsernames

            var allEmails = new List<string> { };

            //get all emails from the database and pass them to allEmails


            var signInIsValid = false;
            while (!signInIsValid)
            {
                Console.WriteLine("SIGN IN");
                Console.WriteLine("Enter your username:");
                string username = Console.ReadLine();
                Console.WriteLine("Enter your password:");
                string password = Console.ReadLine();
                Console.WriteLine("Enter your password again:");
                string repeatedPassword = Console.ReadLine();
                Console.WriteLine("Enter your first name:");
                string firstName = Console.ReadLine();
                Console.WriteLine("Enter your last name:");
                string lastName = Console.ReadLine();
                Console.WriteLine("Enter your email:");
                string email = Console.ReadLine().ToLower();

                var credentialsAvailable = VerifyAvailability(username, email);

                if (password != repeatedPassword)
                {
                    Console.WriteLine("Passwords do not match");
                    signInIsValid = false;
                }
                else if (!credentialsAvailable)
                {
                    Console.WriteLine("Username or email already in use.");
                    signInIsValid = false;
                }
                else if (password.Length < 10)
                {
                    Console.WriteLine("Passwords must be atleast 10 symbols.");
                    signInIsValid = false;
                }
                else if (firstName.Length == 0)
                {
                    Console.WriteLine("First name is required");
                    signInIsValid = false;
                }
                else
                {
                    Console.WriteLine("Signing in...");
                    signInIsValid = true;
                    byte[] salt = PasswordHasher.GenerateSalt();
                    string hashedPassword = PasswordHasher.HashPassword(password, salt);

                    CreateUser.CreateUserAccount(username, hashedPassword, firstName, email, salt, lastName);
                    Console.WriteLine("User account created successfully.");
                    LogIn logIn = new LogIn();
                    logIn.Login();
                }
            }
        }
        ValidResponse VerifyAvailability(string usernameInput, string passwordInput)
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
                        string query = "SELECT Username, Email FROM SecretaryDB.dbo.Users WHERE Username = @Username OR Username = @Email";
                        using (SqlCommand sqlQuery = new SqlCommand(query, connection))
                        {
                            sqlQuery.Parameters.Add("@Username", System.Data.SqlDbType.VarChar, 18).Value = usernameInput;
                            sqlQuery.Parameters.Add("@Email", System.Data.SqlDbType.VarChar, 50).Value = usernameInput;
                            connection.Open();
                            using (SqlDataReader reader = sqlQuery.ExecuteReader())
                            {
                                if (reader.Read())
                                {

                                    var storedUsername = reader["UserId"].ToString();
                                    var storedEmail = reader["UserId"].ToString();
                                    if (storedUsername == usernameInput)
                                    {

                                        return resp;
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
        public class ValidResponse
        {
            public string ResultMessage { get; set; }
            public bool ResultBool { get; set; }
            public ValidResponse(string resultMessage, bool result)
            {
                ResultMessage = resultMessage;
                ResultBool = result;
            }
        }
    }
}
