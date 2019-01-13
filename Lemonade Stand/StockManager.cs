using System;
using System.Collections.Generic;
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
            var selectedItem = -1;
            var done = false;
            var stockItems = _dataManager.Stock.Include(s => s.Product).Include(s => s.Product.Category).ToList();
            if (stockItems.Count > 0) selectedItem = 0;

            while (!done)
            {
                Console.Clear();
                Console.WriteLine("=== STOCK EDITOR ===");
                Console.WriteLine(
                    "Press the up and down arrows to select an item or action, then press left and right arrows to change stock amount.");
                Console.WriteLine($" {(selectedItem == -3 ? ">" : "-")} Create a new category");
                Console.WriteLine($" {(selectedItem == -2 ? ">" : "-")} Create a new product");
                Console.WriteLine($" {(selectedItem == -1 ? ">" : "-")} Exit Stock Editor");
                Console.WriteLine("------------------------------");

                for (var i = 0; i < stockItems.Count; i++)
                {
                    var item = stockItems[i];
                    Console.WriteLine(
                        $" {(selectedItem == i ? ">" : "-")} [{UiUtils.FixStringLength(item.Product.Category.Name, 6)}] {UiUtils.FixStringLength(item.Product.Name, 15)}  < {item.Quantity} >");
                }

                var k = Console.ReadKey(true);

                if (k.Key == ConsoleKey.DownArrow && selectedItem + 1 < stockItems.Count)
                {
                    selectedItem += 1;
                }
                else if (k.Key == ConsoleKey.UpArrow && selectedItem > -3)
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
                    var newStockItem = NewStockEditor();

                    if (newStockItem == null) continue;

                    // Save new stock item to database
                    _dataManager.Stock.Add(newStockItem);
                    _dataManager.SaveChanges();
                    
                    // Reload stock from database
                    stockItems = _dataManager.Stock.Include(s => s.Product).Include(s => s.Product.Category).ToList();
                } else if (k.Key == ConsoleKey.Enter && selectedItem == -3)
                {
                    var newCategory = NewCategoryEditor();

                    if (newCategory == null) continue;

                    // Save new category to database
                    _dataManager.Categories.Add(newCategory);
                    _dataManager.SaveChanges();

                    // Reload stock from database
                    stockItems = _dataManager.Stock.Include(s => s.Product).Include(s => s.Product.Category).ToList();
                }
            }
        }

        public Category NewCategoryEditor()
        {
            Console.Clear();
            Console.WriteLine("=== NEW CATEGORY ===");
            var category = (string)UiUtils.Field("Category Name:");
            return new Category()
            {
                Name = category
            };
        }

        public StockItem NewStockEditor()
        {
            var categories = _dataManager.Categories.ToList();

            if (categories.Count <= 0)
            {
                return null;
            }

            var categoryStrings = new List<string>();

            foreach (var c in categories)
            {
                categoryStrings.Add(c.Name);
            }

            var categoryInput = UiUtils.Menu("Please select a category for the new item", categoryStrings.ToArray());
            var category = _dataManager.Categories.First(c => c.Name == categoryInput);

            var name = (string) UiUtils.Field("Product Name:");
            var price = (float) UiUtils.Field("Product Price:", "float");

            return new StockItem
            {
                Quantity = 0,
                Product = new Product
                {
                    Name = name,
                    Category = category,
                    Price = price
                }
            };
        }
    }
}