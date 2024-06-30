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
                else if (!credentialsAvailable.ResultBool)
                {
                    Console.WriteLine(credentialsAvailable.ResultMessage);
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
        ValidResponse VerifyAvailability(string usernameInput, string emailInput)
        {
            string connectionString = "Server=db.dw154.webglobe.com;Database=program_db;User=samuel_wachtel;Password=zuwxy9-vezveP-vuwcyr;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    using (connection)
                    {
                        string query = "SELECT Username, Email FROM SecretaryDB.dbo.Users WHERE Username = @Username OR Email = @Email";
                        using (SqlCommand sqlQuery = new SqlCommand(query, connection))
                        {
                            connection.Open();
                            sqlQuery.Parameters.Add("@Username", System.Data.SqlDbType.VarChar, 18).Value = usernameInput;
                            sqlQuery.Parameters.Add("@Email", System.Data.SqlDbType.VarChar, 50).Value = usernameInput;
                            connection.Open();
                            using (SqlDataReader reader = sqlQuery.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    var storedUsername = reader["UserId"].ToString();
                                    var storedEmail = reader["UserId"].ToString();
                                    if (storedUsername != usernameInput && storedEmail == emailInput)
                                    {
                                        ValidResponse response = new ValidResponse("Email is already used.", false);
                                        return response;
                                    }
                                    else if (storedEmail != emailInput && storedUsername == usernameInput)
                                    {
                                        ValidResponse response = new ValidResponse("Username is already used.", false);
                                        return response;
                                    }
                                    else if (storedUsername != usernameInput && storedEmail != emailInput)
                                    {
                                        ValidResponse response = new ValidResponse("ok", true);
                                        return response;
                                    }
                                    else if (storedUsername == usernameInput && storedEmail == emailInput)
                                    {
                                        
                                    }
                                    {
                                        ValidResponse response = new ValidResponse("Username and email are already used.", false);
                                        return response;
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    ValidResponse errorResponse = new ValidResponse("An error occured. Please try again later.", false);
                    return errorResponse;
                }
                ValidResponse errorResponse2 = new ValidResponse("An error occured. Please try again later.", false);
                return errorResponse2;
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
