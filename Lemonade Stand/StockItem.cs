namespace Lemonade_Stand
{
    public class StockItem
    {
        /// <summary>
        ///     The ID of the product in stock
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     The product in the stock
        /// </summary>
        public Product Product { get; set; }

        /// <summary>
        ///     The amount of the product in the stock
        /// </summary>
        public int Quantity { get; set; }
    }
}