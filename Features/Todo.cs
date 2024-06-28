using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Secretare.Features
{
    class Todo
    {
        public void ReadTodos()
        {
            Console.WriteLine("Enter a command (new, list, edit, delete):");
            string input = Console.ReadLine();

            var continueInput = "";
            switch (input.ToLower())
            {
                case "new":
                    CreateTodo();
                    Console.WriteLine("Continue?");
                    continueInput = Console.ReadLine().ToLower();
                    if (continueInput == "no")
                        break;
                    else
                        ReadTodos();
                    break;
                case "list":
                    ListTodos();
                    Console.WriteLine("Continue?");
                    continueInput = Console.ReadLine().ToLower();
                    if (continueInput == "no")
                        break;
                    else
                        ReadTodos();
                    break;
                case "edit":
                    EditTodo();
                    Console.WriteLine("Continue?");
                    continueInput = Console.ReadLine().ToLower();
                    if (continueInput == "no")
                        break;
                    else
                        ReadTodos();
                    break;
                case "delete":
                    DeleteTodo();
                    Console.WriteLine("Continue?");
                    continueInput = Console.ReadLine().ToLower();
                    if (continueInput == "no")
                        break;
                    else
                        ReadTodos();
                    break;
                default:
                    Console.WriteLine("Invalid command");
                    ReadTodos();
                    break;
            }
        }
        public void CreateTodo()
        {
            Console.WriteLine("Enter a new todo:");
            string todo = Console.ReadLine();
            Console.WriteLine("Enter a due date:");
            string dueDate = Console.ReadLine();
            Console.WriteLine("Enter a priority (high, medium, low):");
            string priority = Console.ReadLine();
            Console.WriteLine("Enter a status (open, in progress, done):");
            string status = Console.ReadLine();
            Console.WriteLine("Enter a category:");
            string category = Console.ReadLine();
            Console.WriteLine("Enter a note:");
            string note = Console.ReadLine();
        }
        public void ListTodos()
        {
            Console.WriteLine("List of todos:");
        }
        public void EditTodo()
        {
            Console.WriteLine("Edit a todo:");
        }
        public void DeleteTodo()
        {
            Console.WriteLine("Delete a todo:");
        }
    }
}
