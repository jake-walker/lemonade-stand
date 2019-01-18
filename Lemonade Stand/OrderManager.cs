using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Lemonade_Stand
{
    class OrderManager
    {
        private readonly DataManager _dataManager;

        public float cash = 0.0f;
        // *** get money from database at beginning of the day

        public OrderManager(DataManager dataManager)
        {
            _dataManager = dataManager;
        }

        public Transaction NewOrder()
        {
            // Create a new transaction object.
            var t = new Transaction();
            // Get all the categories from the database.
            var categories = _dataManager.Categories.Select(c => c.Name).ToList();
            // Add a finish option to the list.
            categories.Add("<FINISH>");

            // Create a list for storing the products in the user's basket.
            var basket = new List<Product>();

            // Create a variable for storing whether the order is complete.
            var done = false;

            while (!done)
            {
                // Prompt the user for which category of item that they would like to pick (e.g Breakfast)
                var categoryResponse = UiUtils.Menu("Pick the category of the item that you would like to order.", categories.ToArray());

                // If the user selects to finish.
                if (categoryResponse == "<FINISH>")
                {
                    // Set done to true to break out of the while loop.
                    done = true;
                }
                // If the user selected a category
                else
                {
                    // Get all the products from the database which have a category of the selected category.
                    var categoryProducts =
                        _dataManager.Products.Include(p => p.Category).Where(p => p.Category.Name == categoryResponse).Select(p => p.Name).ToList();
                    // Add a back option to the products selection.
                    categoryProducts.Add("<BACK>");

                    // Ask the user for which product they would like to add.
                    var productResponse =
                        UiUtils.Menu("Pick the product that you would like to order.", categoryProducts.ToArray());

                    // If the user selected a product
                    if (productResponse != "<BACK>")
                    {
                        // Add the product to the basket
                        basket.Add(_dataManager.Products.Include(p => p.Category).FirstOrDefault(p => p.Name == productResponse && p.Category.Name == categoryResponse));
                    }

                    // Now we are going back to the top of the loop.
                }
            }

            // Check and add stock
            var newBasket = CheckStock(basket);

            // If the user now has nothing in their basket
            if (newBasket.Count <= 0)
            {
                // Return nothing, as the order was cancelled.
                return null;
            }
            
            basket = newBasket;

            Console.Clear();

            // Print out the basket
            UiUtils.Print($"You have {basket.Count} items in your basket:", "Primary");
            foreach (var p in basket)
            {
                UiUtils.Print($" - {p.Name} (£{p.Price:F2})", "Secondary");
            }

            // Get the total price of the basket
            var total = basket.Sum(p => p.Price);
            // Print our the total
            UiUtils.Print($"Your order is has a total of (£{total:F2})", "Primary");

            // Takes the customers payment amount and calculates change
            Console.WriteLine("Enter amount paid to confirm and continue:");
            var payment = (float)UiUtils.Field("Payment:", "float", biggerThan:total); 
            var change = payment - total;

            // Prints change and adds paid amount to the total amount of cash
            Console.WriteLine($"Your change is {change}");
            cash += total;

            // Ask the user to press enter to exit or the window would disappear too quickly to read.
            UiUtils.Print("Press enter to confirm", "Muted");
            Console.ReadLine();

            // Add the user's basket to the transaction object
            t.Products = basket;

            // Add the transaction to the database
            _dataManager.Transactions.Add(t);
            _dataManager.SaveChanges();

            // Return the transaction object.
            return t;
        }

        /// <summary>
        /// Check the database for the stock for each of the inputs. If there is not enough stock, the user is asked if they want to still keep the item.
        /// </summary>
        /// <param name="items">A list of items in the user's basket</param>
        /// <returns>A list of items in stock</returns>
        private List<Product> CheckStock(List<Product> items)
        {
            // Set a variable for storing the output of stocked items.
            var newBasket = new List<Product>();

            // Tell the user that we are checking stock as it might take a while.
            UiUtils.Print("Checking Stock...", "Primary");

            // For each of the items in the user's basket
            foreach (var item in items)
            {
                // Find the stock in the database where the item is the item in the basket.
                var stock = _dataManager.Stock.Include(i => i.Product).FirstOrDefault(i => i.Product == item);

                // If there isn't any stock
                if (stock == null || stock.Quantity <= 0)
                {
                    // Ask the user if they still want to complete the order
                    var response =
                        (bool)UiUtils.Field(
                            $"There isn't enough {stock.Product.Name} in stock. Would you like to remove this item and continue your order?", "boolean");

                    // If they want to complete the order still
                    if (response)
                    {
                        // Remove the item from the basket
                        items.Remove(stock.Product);
                    }
                    else
                    {
                        // Return an empty list
                        return newBasket;
                    }
                }
                else
                {
                    // Remove one from the stock
                    stock.Quantity -= 1;
                    // Update the stock item in the database
                    _dataManager.Update(stock);
                    // Save the database
                    _dataManager.SaveChanges();
                    // Add the stock item to the database.
                    newBasket.Add(stock.Product);
                }
            }

            return newBasket;
        }
    }
}
