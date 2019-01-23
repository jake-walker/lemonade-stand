namespace Lemonade_Stand
{
    public class Product
    {
        /// <summary>
        ///     Product ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     Product Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Product Category (e.g food, drink)
        /// </summary>
        public Category Category { get; set; }

        /// <summary>
        ///     Product Price
        /// </summary>
        public decimal Price { get; set; }
    }
}