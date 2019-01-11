using System;

namespace Lemonade_Stand
{
    class Program
    {
        private StockManager _stockManager = new StockManager();

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the Lemonade Stand!");
            
            Console.WriteLine("Press enter to exit...");
            Console.ReadLine();
        }
    }
}

