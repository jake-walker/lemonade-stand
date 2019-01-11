using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemonade_Stand
{
    public class StockItem
    {
        /// <summary>
        /// The product in the stock
        /// </summary>
        public Product Product { get; set; }

        /// <summary>
        /// The amount of the product in the stock
        /// </summary>
        public int Quantity { get; set; }
    }
}
