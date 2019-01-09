using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemonade_Stand
{
    class Transaction
    {
        public int Id { get; set; }
        public List<Product> Order { get; set; }
    }
}
