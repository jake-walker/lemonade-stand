using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemonade_Stand
{
    public class OrderInfo
    {
        /// <summary>
        /// The ID of the order
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The total amount of the order
        /// </summary>
        public decimal Total { get; set; }
    }
}
