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
            var t = new Transaction();
            var categories = _dataManager.Categories.Select(c => c.Name).ToList();
            categories.Add("<FINISH>");

            var basket = new List<Product>();

            var done = false;

            while (!done)
            {
                var categoryResponse = UiUtils.Menu("Pick the category of the item that you would like to order.", categories.ToArray());

                if (categoryResponse == "<FINISH>")
                {
                    done = true;
                }
                else
                {
                    var categoryProducts =
                        _dataManager.Products.Include(p => p.Category).Where(p => p.Category.Name == categoryResponse).Select(p => p.Name).ToList();
                    categoryProducts.Add("<BACK>");

                    var productResponse =
                        UiUtils.Menu("Pick the product that you would like to order.", categoryProducts.ToArray());

                    if (productResponse != "<BACK>")
                    {
                        basket.Add(_dataManager.Products.Include(p => p.Category).FirstOrDefault(p => p.Name == productResponse && p.Category.Name == categoryResponse));
                    }
                }
            }

            // Check stock
            var newBasket = CheckStock(basket);
            if (newBasket.Count <= 0)
            {
                return null;
            }

            basket = newBasket;

            Console.Clear();
            UiUtils.Print($"You have {basket.Count} items in your basket:", "Primary");
            foreach (var p in basket)
            {
                UiUtils.Print($" - {p.Name} (£{p.Price:F2})", "Secondary");
            }

            var total = basket.Sum(p => p.Price);
            UiUtils.Print($"Your order is has a total of (£{total:F2})", "Primary");

            Console.WriteLine("Enter amount paid to confirm and continue:");
            var payment = (float)UiUtils.Field("Payment:", "float");
            cash -= payment;

            t.Order = basket;

            _dataManager.Transactions.Add(t);

            return t;
        }

        private List<Product> CheckStock(List<Product> items)
        {
            var newBasket = new List<Product>();

            UiUtils.Print("Checking Stock...", "Primary");

            foreach (var item in items)
            {
                var stock = _dataManager.Stock.Include(i => i.Product).FirstOrDefault(i => i.Product == item);

                if (stock == null || stock.Quantity <= 0)
                {
                    var response =
                        (bool)UiUtils.Field(
                            $"There isn't enough {stock.Product.Name} in stock. Would you like to remove this item and continue your order?", "boolean");

                    if (response)
                    {
                        items.Remove(stock.Product);
                    }
                    else
                    {
                        return new List<Product>();
                    }
                }
                else
                {
                    stock.Quantity -= 1;
                    _dataManager.Update(stock);
                    _dataManager.SaveChanges();
                    newBasket.Add(stock.Product);
                }
            }

            return newBasket;
        }
    }
}
