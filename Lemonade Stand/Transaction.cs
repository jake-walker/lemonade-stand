﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemonade_Stand
{
    public class Transaction
    {
        /// <summary>
        /// Transaction ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// List of Products that the customer has ordered.
        /// </summary>
        public List<Product> Order { get; set; }
    }
}
