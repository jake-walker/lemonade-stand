using System;

namespace Lemonade_Stand
{
    class Program
    {
        private StockManager _stockManager = new StockManager();

        static void Main(string[] args)
        { 
            var creds = Get_creds("Please enter your worker ID and password");      
            // creds.Item1 is the username and the creds.Item2 is the password
            var flag = Validate_creds(creds.Item1, creds.Item2);
            while (flag == false)
            {
                creds = Get_creds("Incorrect ID or password, please retry");                
                flag = Validate_creds(creds.Item1, creds.Item2);
            }
            // **This will have to be compared to the database later
            if (creds.Item1 != "1234" || creds.Item2 != "1234")
            { 
                creds = Get_creds("Incorrect ID or password, please retry");
                flag = Validate_creds(creds.Item1, creds.Item2);
            } 

            Console.WriteLine("Press enter to exit...");
            Console.ReadLine();
        }

        /// <summary>
        /// Asks the server for their the username and password
        /// </summary>
        /// <param name="message"> The display message for the server </param>
        /// <returns> Tuple of the username and password inputted by the server </returns>
        static Tuple<string,string> Get_creds(string message)
        {
            Console.WriteLine(message);
            Console.Write("ID:");
            var id = Console.ReadLine();
            Console.Write("Password:");
            var pass = Console.ReadLine();
            return (Tuple.Create(id, pass));
        }

        /// <summary>
        /// Validates username and password - length and checks null
        /// </summary>
        /// <param name="id"> Workers ID </param>
        /// <param name="pass"> Workers password </param>
        /// <returns> Result of validation </returns>
        static bool Validate_creds(string id, string pass)
        {
            if (id != null && pass != null)
            {
                if (id.Length == 4 && pass.Length == 4)
                {
                    return (true);
                }
                else
                {
                    return (false);
                }
            }
            else
            {
                return (false);
            }
        }
            
    }
}

