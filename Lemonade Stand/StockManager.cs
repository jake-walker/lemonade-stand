using System;
using System.Linq;
using System.Threading;
using Microsoft.EntityFrameworkCore;

namespace Lemonade_Stand
{
    public class StockManager
    {
        /// <summary>
        ///     The data manager object to use for storing/retrieving data
        /// </summary>
        private readonly DataManager _dataManager;

        public StockManager(DataManager dataManager)
        {
            _dataManager = dataManager;
        }

        public void StockEditor()
        {
            var selectedItem = -2;
            var done = false;
            var stockItems = _dataManager.Stock.Include(s => s.Product).ToList();

            while (!done)
            {
                Console.Clear();
                Console.WriteLine("=== STOCK EDITOR ===");
                Console.WriteLine(
                    "Press the up and down arrows to select an item or action, then press left and right arrows to change stock amount.");
                Console.WriteLine($" {(selectedItem == -2 ? ">" : "-")} Create a new item");
                Console.WriteLine($" {(selectedItem == -1 ? ">" : "-")} Exit Stock Editor");
                Console.WriteLine("------------------------------");

                for (var i = 0; i < stockItems.Count; i++)
                {
                    var item = stockItems[i];
                    Console.WriteLine(
                        $" {(selectedItem == i ? ">" : "-")} {item.Product.Name.Substring(0, item.Product.Name.Length < 10 ? item.Product.Name.Length : 10).PadRight(10)}    < {item.Quantity} >");
                }

                var k = Console.ReadKey(true);

                if (k.Key == ConsoleKey.DownArrow && selectedItem + 1 < stockItems.Count)
                {
                    selectedItem += 1;
                }
                else if (k.Key == ConsoleKey.UpArrow && selectedItem > -2)
                {
                    selectedItem -= 1;
                }
                else if (k.Key == ConsoleKey.LeftArrow && stockItems[selectedItem].Quantity > 0)
                {
                    stockItems[selectedItem].Quantity -= 1;
                }
                else if (k.Key == ConsoleKey.RightArrow)
                {
                    stockItems[selectedItem].Quantity += 1;
                }
                else if (k.Key == ConsoleKey.Enter && selectedItem == -1)
                {
                    _dataManager.Stock.UpdateRange(stockItems);
                    _dataManager.SaveChanges();
                    return;
                }
                else if (k.Key == ConsoleKey.Enter && selectedItem == -2)
                {
                    stockItems.Add(NewStockEditor());
                }
            }
        }

        public StockItem NewStockEditor()
        {
            var item = new StockItem
            {
                Quantity = 0
            };

            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== NEW STOCK ITEM ===");
                Console.Write(" Product Name:       ");
                var name = Console.ReadLine();
                Console.Write(" Product Category:   ");
                var category = Console.ReadLine();
                Console.Write(" Product Price:      ");
                var price = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(name))
                {
                    Console.WriteLine("You must type a name!");
                }
                else if (string.IsNullOrWhiteSpace(category))
                {
                    Console.WriteLine("You must type a category!");
                }
                else if (!float.TryParse(price, out var f))
                {
                    Console.WriteLine("You must type a valid price!");
                }
                else
                {
                    var p = new Product
                    {
                        Name = name,
                        Category = category,
                        Price = f
                    };
                    item.Product = p;
                    return item;
                }

                Thread.Sleep(2000);
            }
        }
    }
}