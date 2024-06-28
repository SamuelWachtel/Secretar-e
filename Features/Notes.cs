using System.Data.SqlClient;
using Secretare.LoggingIn;

namespace Secretare.Features
{
    class Notes
    {

        public void ReadNotes()
        {
            SqlConnectionStringBuilder csb = new SqlConnectionStringBuilder();
            csb.DataSource = @"(localDB)\MSSQLSERVER02";
            csb.InitialCatalog = "Secretary";
            csb.IntegratedSecurity = true;
            var connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\samuel.wachtel\source\repos\Secretar.io\Secretare\SecretaryDB.mdf;Integrated Security=True";
            var continueString = true;
                while (continueString)
                        { 
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                    try
                {
                    //connection.Open();
                    Console.WriteLine("Connection successful");

                    Console.WriteLine("Enter a command (new, list, edit, delete):");

                    Console.WriteLine($"Logged in user ID: {LogIn.LoggedInUserId}");

                        string input = Console.ReadLine();

                    var continueInput = "";
                    switch (input.ToLower())
                    {
                        case "new":
                            CreateNote(connection);
                                Console.WriteLine("Continue?");
                                continueInput = Console.ReadLine().ToLower();
                                if (continueInput == "no")
                                    continueString = false;
                                else
                                    continueString = true;
                                break;
                        case "list":
                            ListNotes(connection);
                                Console.WriteLine("Continue?");
                                continueInput = Console.ReadLine().ToLower();
                                if (continueInput == "no")
                                    continueString = false;
                                else
                                    continueString = true;
                                break;
                        case "edit":
                            EditNote(connection);
                                Console.WriteLine("Continue?");
                                continueInput = Console.ReadLine().ToLower();
                                if (continueInput == "no")
                                    continueString = false;
                                else
                                    continueString = true;
                                break;
                        case "delete":
                            DeleteNote(connection);
                                Console.WriteLine("Continue?");
                                continueInput = Console.ReadLine().ToLower();
                                if (continueInput == "no")
                                    continueString = false;
                                else
                                    continueString = true;
                                break;
                            case "truncate":
                                TruncateNotes(connection);
                                Console.WriteLine("Continue?");
                                continueInput = Console.ReadLine().ToLower();
                                if (continueInput == "no")
                                    continueString = false;
                                else
                                    continueString = true;
                                break;
                            default:
                            Console.WriteLine("Invalid command");
                            break;
                    }
                    //connection.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Connection failed: {ex.Message}");
                }
                } 
            }
        }

        public static void CreateNote(SqlConnection connection)
        {
            try
            {
                using (connection)
                {
                    string query = "INSERT INTO SecretaryDB.dbo.Notes (UserId, Title, Content) VALUES (@UserId, @Title, @Content)";
                    using (SqlCommand sqlQuery = new SqlCommand(query))
                    {
                        sqlQuery.Connection = connection;
                        Console.WriteLine("Enter note title:");
                        string title = Console.ReadLine();
                        Console.WriteLine("Enter note content:");
                        string content = Console.ReadLine();
                        sqlQuery.Parameters.Add("@Title",System.Data.SqlDbType.VarChar,30).Value = title;
                        sqlQuery.Parameters.Add("@Content", System.Data.SqlDbType.VarChar, 30).Value = content;
                        sqlQuery.Parameters.Add("@UserId", System.Data.SqlDbType.Int).Value = LogIn.LoggedInUserId;
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

        public static void ListNotes(SqlConnection connection)
        {
            try
            {
                string query = "SELECT * FROM SecretaryDB.dbo.Notes WHERE UserId = @UserId";
                using (SqlCommand sqlQuery = new SqlCommand(query, connection))
                {
                    sqlQuery.Parameters.Add("@UserId", System.Data.SqlDbType.Int).Value = LogIn.LoggedInUserId;
                    connection.Open();
                    using (SqlDataReader reader = sqlQuery.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"NoteID: {reader["NoteId"]}, UserId: {reader["UserId"]}, Title: {reader["Title"]}, Content: {reader["Content"]}, Data Created: {reader["CreatedDate"]}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                connection.Close();
            }
        }

        public static void EditNote(SqlConnection connection)
        {
            try
            {
                Console.WriteLine("Enter NoteID to edit:");
                int id = int.Parse(Console.ReadLine());

                Console.WriteLine("Enter new note title:");
                string title = Console.ReadLine();
                Console.WriteLine("Enter new note content:");
                string content = Console.ReadLine();

                string query = "UPDATE SecretaryDB.dbo.Notes SET Title = @Title, Content = @Content WHERE NoteId = @ID AND UserId = @UserId";
                using (SqlCommand sqlQuery = new SqlCommand(query, connection))
                {
                    connection.Open();
                    sqlQuery.Parameters.AddWithValue("@ID", id);
                    sqlQuery.Parameters.AddWithValue("@Title", title);
                    sqlQuery.Parameters.AddWithValue("@Content", content);
                    sqlQuery.Parameters.AddWithValue("@UserId", LogIn.LoggedInUserId);

                    int rowsAffected = sqlQuery.ExecuteNonQuery();
                    Console.WriteLine($"{rowsAffected} row(s) updated.");
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public static void DeleteNote(SqlConnection connection)
        {
            try
            {
                Console.WriteLine("Enter note ID to delete:");
                int id = int.Parse(Console.ReadLine());

                string query = "DELETE FROM SecretaryDB.dbo.Notes WHERE NoteId = @ID AND UserId = @UserId";
                using (SqlCommand sqlQuery = new SqlCommand(query, connection))
                {
                    connection.Open();
                    sqlQuery.Parameters.AddWithValue("@ID", id);
                    sqlQuery.Parameters.AddWithValue("@UserId", LogIn.LoggedInUserId);

                    int rowsAffected = sqlQuery.ExecuteNonQuery();
                    Console.WriteLine($"{rowsAffected} row(s) deleted.");
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public static void TruncateNotes(SqlConnection connection)
        {
            try
            {
                string query = "TRUNCATE TABLE SecretaryDB.dbo.Notes";
                using (SqlCommand sqlQuery = new SqlCommand(query, connection))
                {
                    connection.Open();
                    sqlQuery.ExecuteNonQuery();
                    Console.WriteLine("Table truncated.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                connection.Close();
            }
        }
    }
}