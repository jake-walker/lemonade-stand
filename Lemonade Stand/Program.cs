using System;
using System.Threading;

namespace Lemonade_Stand
{
    internal class Program
    {
        private static readonly DataManager DataManager = new DataManager();

        private static readonly UserManager UserManager = new UserManager(DataManager);
        private static readonly StockManager StockManager = new StockManager(DataManager);

        private static void Main(string[] args)
        {
            var done = false;
            while (!done)
            {
                // Fetch the credentials from the user
                var (username, password) = GetCredentials();

                // If the login is successful, break the loop
                done = UserManager.Login(username, password);

                // If the login wasn't successful, print out an error message.
                if (!done) Console.WriteLine("The entered credentials were incorrect. Please try again.");
            }

            // Get the current user from the manager.
            var user = UserManager.CurrentUser;
            // Say which user is logged in.
            Console.WriteLine($"Welcome worker {user.Id}!");

            Thread.Sleep(1000);

            var action = "";

            while (action != "Logout & Quit")
            {
                action = Menu("What would you like to do?", "New Order", "Stock Management", "Logout & Quit");

                if (action == "New Order")
                {
                    // TODO: Place order
                }
                else if (action == "Stock Management")
                {
                    StockManager.StockEditor();
                }
            }

            // Ask the user to press enter to exit or the window would disappear too quickly to read.
            Console.WriteLine("\nPress enter to exit...");
            Console.ReadLine();
        }

        /// <summary>
        ///     Asks the server for their the username and password
        /// </summary>
        /// <param name="message">The display message for the worker</param>
        /// <returns>Tuple of the username and password inputted by the worker</returns>
        private static Tuple<string, string> GetCredentials(string message = "Please enter your worker ID and password")
        {
            // Write out the prompt message
            Console.WriteLine(message);

            // Ask for the ID and the password
            Console.Write(" ID:       ");
            var id = Console.ReadLine();
            Console.Write(" Password: ");

            // Get the password (but mask what the user types)
            var pass = ReadPassword();

            // Return the user's ID and password in a tuple.
            return Tuple.Create(id, pass);
        }

        /// <summary>
        ///     Get the users input, masking the input (for passwords)
        /// </summary>
        /// <returns>A string of what the user types</returns>
        private static string ReadPassword()
        {
            var done = false;
            var output = "";

            while (!done)
            {
                // Get the key that the user presses, intercepting so that it isn't shown to the user.
                var k = Console.ReadKey(true);

                switch (k.Key)
                {
                    // If the key is enter, break the loop
                    case ConsoleKey.Enter:
                        done = true;
                        break;
                    // If the key is backspace and the user has typed something...
                    case ConsoleKey.Backspace when output.Length > 0:
                        // Remove the last character from the output string
                        output = output.Substring(0, output.Length - 1);
                        // Go back a character, print a space then go back another character to ensure that the character is replaced by a space.
                        Console.Write("\b \b");
                        break;
                    // For any other key...
                    default:
                        // Add the key's character to the output
                        output += k.KeyChar;
                        // Print an asterisk to the console
                        Console.Write("*");
                        break;
                }
            }

            Console.WriteLine("");
            return output;
        }

        /// <summary>
        ///     Display a menu and handle user inputs
        /// </summary>
        /// <param name="prompt">The message to display to the user</param>
        /// <param name="items">The items that the user will be able to pick</param>
        /// <returns>The item that the user chose</returns>
        private static string Menu(string prompt, params string[] items)
        {
            var current = 0;
            var done = false;

            while (!done)
            {
                Console.Clear();
                Console.WriteLine($"{prompt}");
                Console.WriteLine(
                    "Press the up and down arrow keys to change items and the enter key to confirm a selection.");

                for (var i = 0; i < items.Length; i++) Console.WriteLine($" {(i == current ? '>' : '-')} {items[i]}");

                var k = Console.ReadKey(true);

                switch (k.Key)
                {
                    case ConsoleKey.UpArrow when current > 0:
                        current -= 1;
                        break;
                    case ConsoleKey.DownArrow when current + 1 < items.Length:
                        current += 1;
                        break;
                    case ConsoleKey.Enter:
                        done = true;
                        break;
                }
            }

            return items[current];
        }
    }
}