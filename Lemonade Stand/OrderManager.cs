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

        public decimal cash = 0.0M;
        // *** get money from database at beginning of the day

        public OrderManager(DataManager dataManager)
        {
            _dataManager = dataManager;
        }

        public void OrderViewer()
        {
            var orderIds = _dataManager.OrderInfos.Select(o => o.Id).ToList();
            orderIds.Add("<BACK>");

            while (true)
            {
                var response = UiUtils.Menu("Select an Order ID:", orderIds.ToArray());
                if (response == "<BACK>")
                {
                    return;
                }
                else
                {
                    var orderInfo = _dataManager.OrderInfos.FirstOrDefault(o => o.Id == response);
                    var transactions = _dataManager.Transactions.Where(t => t.OrderId == response).Include(t => t.Product).ToList();

                    if (orderInfo == null && !transactions.Any())
                    {
                        UiUtils.Print("That order is invalid!", "Danger");
                        UiUtils.Print("Press enter to continue...", "Muted");
                        Console.ReadLine();
                        return;
                    }

                    Console.Clear();
                    UiUtils.Print($"== ORDER {orderInfo.Id} ==", "Primary");
                    UiUtils.Print($"Order Total:         £{orderInfo.Total:F2}", "Secondary");

                    foreach (var item in transactions)
                    {
                        UiUtils.Print("====================================================", "Muted");
                        UiUtils.Print($"Item ID:             {item.Product.Id}", "Secondary");
                        UiUtils.Print($"Item Name:           {item.Product.Name}", "Secondary");
                        UiUtils.Print($"Item Price:          £{item.Product.Price:F2}", "Secondary");
                        UiUtils.Print($"Item Category:       {item.Product.Category}", "Secondary");
                    }

                    UiUtils.Print("Press enter to continue...", "Muted");
                    Console.ReadLine();
                }
            }
        }

        public OrderInfo StoreOrder(List<Product> products)
        {
            var orderId = Guid.NewGuid().ToString();
            var info = new OrderInfo {Id = orderId, Total = products.Sum(p => p.Price)};
            var transactions = new List<Transaction>();

            foreach (var product in products)
            {
                transactions.Add(new Transaction
                {
                    OrderId = orderId,
                    Product = product
                });
            }

            _dataManager.OrderInfos.Add(info);
            _dataManager.Transactions.AddRange(transactions);
            _dataManager.SaveChanges();

            return info;
        }

        public OrderInfo NewOrder()
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
            var payment = (decimal)UiUtils.Field("Payment:", "decimal"); // ** needs to be more than the total
            var change = payment - total;
            //** print change due
            cash += payment;

            var orderInfo = StoreOrder(basket);

            UiUtils.Print($"Thank you for your order! Your order reference is {orderInfo.Id}.", "Muted");
            UiUtils.Print("Press enter to continue...", "Muted");
            Console.ReadLine();

            // Return the transaction object.
            return orderInfo;
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
                        //items.Remove(stock.Product);
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
