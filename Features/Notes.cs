using System;
using System.Data.SqlClient;
using System.IO;

namespace Secretare.Features
{
    class Notes
    {
        public static string NoteDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Secretare\Notes";

        public void ReadInput()
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
                    string query = "INSERT INTO SecretaryDB.dbo.Notes (Title, Content) VALUES (@Title, @Content)";
                    using (SqlCommand sqlQuery = new SqlCommand(query))
                    {
                        sqlQuery.Connection = connection;
                        Console.WriteLine("Enter note title:");
                        string title = Console.ReadLine();
                        Console.WriteLine("Enter note content:");
                        string content = Console.ReadLine();
                        sqlQuery.Parameters.Add("@Title",System.Data.SqlDbType.VarChar,30).Value = title;
                        sqlQuery.Parameters.Add("@Content", System.Data.SqlDbType.VarChar, 30).Value = content;
                        connection.Open();
                        sqlQuery.ExecuteNonQuery();
                        connection.Close();
                    }
                }

                /*

                    Console.WriteLine("Enter note title:");
                string title = Console.ReadLine();
                Console.WriteLine("Enter note content:");
                string content = Console.ReadLine();
                    connection.Open();

                //string query = "INSERT INTO SecretaryDB.dbo.Notes (Title, Content) VALUES (@Title, @Content)";
                using (SqlCommand sqlQuery = new SqlCommand(query, connection))
                {
                    sqlQuery.Parameters.AddWithValue("@Title", title);
                    sqlQuery.Parameters.AddWithValue("@Content", content);

                    
                    connection.Close();
                }
                */
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
                string query = "SELECT * FROM SecretaryDB.dbo.Notes";
                using (SqlCommand sqlQuery = new SqlCommand(query, connection))
                {
                connection.Open();
                    using (SqlDataReader reader = sqlQuery.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"Title: {reader["Title"]}, Content: {reader["Content"]}");
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
                Console.WriteLine("Enter note ID to edit:");
                int id = int.Parse(Console.ReadLine());

                Console.WriteLine("Enter new note title:");
                string title = Console.ReadLine();
                Console.WriteLine("Enter new note content:");
                string content = Console.ReadLine();

                string query = "UPDATE Notes SET Title = @Title, Content = @Content WHERE ID = @ID";
                using (SqlCommand sqlQuery = new SqlCommand(query, connection))
                {
                    sqlQuery.Parameters.AddWithValue("@ID", id);
                    sqlQuery.Parameters.AddWithValue("@Title", title);
                    sqlQuery.Parameters.AddWithValue("@Content", content);

                    int rowsAffected = sqlQuery.ExecuteNonQuery();
                    Console.WriteLine($"{rowsAffected} row(s) updated.");
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

                string query = "DELETE FROM Notes WHERE ID = @ID";
                using (SqlCommand sqlQuery = new SqlCommand(query, connection))
                {
                    sqlQuery.Parameters.AddWithValue("@ID", id);

                    int rowsAffected = sqlQuery.ExecuteNonQuery();
                    Console.WriteLine($"{rowsAffected} row(s) deleted.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}