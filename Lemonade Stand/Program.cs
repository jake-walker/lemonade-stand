using System;
using System.Threading;

namespace Lemonade_Stand
{
    internal class Program
    {
        private static readonly DataManager DataManager = new DataManager();

        private static readonly UserManager UserManager = new UserManager(DataManager);
        private static readonly StockManager StockManager = new StockManager(DataManager);
        private static readonly OrderManager OrderManager = new OrderManager(DataManager);

        private static void Main(string[] args)
        {
            var done = false;
            while (!done)
            {
                // Fetch the credentials from the user
                var credentials = GetCredentials();

                // If the login is successful, break the loop
                done = UserManager.Login(credentials.Item1, credentials.Item2);

                // If the login wasn't successful, print out an error message.
                if (!done) UiUtils.Print("The entered credentials were incorrect. Please try again.", "Danger");
            }

            // Get the current user from the manager.
            var user = UserManager.CurrentUser;
            // Say which user is logged in.
            UiUtils.Print($"Welcome worker {user.Id}!", "Secondary");

            Thread.Sleep(1000);

            var action = "";

            while (action != "Logout & Quit")
            {
                action = UiUtils.Menu("What would you like to do?", "New Order", "Stock Management", "Logout & Quit");

                if (action == "New Order")
                {
                    var t = OrderManager.NewOrder();
                    Console.WriteLine(t);
                }
                else if (action == "Stock Management")
                {
                    StockManager.StockEditor();
                }
            }

            // Ask the user to press enter to exit or the window would disappear too quickly to read.
            UiUtils.Print("\nPress enter to exit...", "Muted");
            Console.ReadLine();
        }

        /// <summary>
        ///     Asks the server for their the username and password
        /// </summary>
        /// <param name="message">The display message for the worker</param>
        /// <returns>Tuple of the username and password inputted by the worker</returns>
        private static Tuple<int, string> GetCredentials(string message = "Please enter your worker ID and password")
        {
            // Write out the prompt message
            UiUtils.Print(message, "Primary");

            // Ask for the ID and the password
            var id = (int)UiUtils.Field("Worker Id:", "int");
            var pass = (string)UiUtils.Field("Password", password:true);

            // Return the user's ID and password in a tuple.
            return Tuple.Create(id, pass);
        }
    }
}