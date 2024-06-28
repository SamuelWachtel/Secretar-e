using Secretare.Features;
using Secretary;
using System;
using Secretare.LoggingIn;

namespace Secretary
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Secretare!");
            Console.WriteLine("Enter a command (login, signin):");
            string input = Console.ReadLine();
            switch (input.ToLower())
            {
                case "login":
                    LogIn login = new LogIn();
                    login.Login();
                    break;
                case "signin":
                    SignIn signin = new SignIn();
                    signin.SignInNewUser();
                    break;
                default:
                    Console.WriteLine("Invalid command");
                    break;
            }
            Notes notes = new Notes();
            notes.ReadNotes();
        }
    }
}