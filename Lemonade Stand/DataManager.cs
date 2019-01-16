using System;
using System.Diagnostics;
using System.IO;
using Microsoft.EntityFrameworkCore;

namespace Lemonade_Stand
{
    public sealed class DataManager : DbContext
    {
        private static bool _created;

        public DataManager()
        {
            if (_created) return;
            _created = true;
            Database.EnsureCreated();
        }


        /// <summary>
        ///     A database set for storing stock information.
        /// </summary>
        public DbSet<StockItem> Stock { get; set; }

        /// <summary>
        ///     A database set for storing transaction information.
        /// </summary>
        public DbSet<Transaction> Transactions { get; set; }

        /// <summary>
        ///     A database set for storing worker information.
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        ///     A database set for storing products.
        /// </summary>
        public DbSet<Product> Products { get; set; }

        /// <summary>
        /// A database set for storing categories.
        /// </summary>
        public DbSet<Category> Categories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionBuilder)
        {
            // Get the user's documents and add on the name of the database file.
            var directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "LemonadeStand.sqlite");
            Debug.WriteLine($"Using {directory} for database storage.");

            // Set the path to save the database.
            optionBuilder.UseSqlite($"Data Source={directory}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Default the ID for Transaction to an auto-incremented value.
            modelBuilder.Entity<Transaction>().Property(t => t.Id).ValueGeneratedOnAdd();

            // Add a default user if the database has just been created.
            modelBuilder.Entity<User>().HasData(new User
            {
                Id = 1,
                Administrator = true,
                Password = "Password"
            });
            // Default the ID for User to an auto-incremented value.
            modelBuilder.Entity<User>().Property(u => u.Id).ValueGeneratedOnAdd();

            // Default the ID for Product to an auto-incremented value.
            modelBuilder.Entity<Product>().Property(p => p.Id).ValueGeneratedOnAdd();

            // Default the ID for Category to an auto-incremented value.
            modelBuilder.Entity<Category>().Property(c => c.Id).ValueGeneratedOnAdd();
            
            // Default the ID for StockItem to an auto-incremented value.
            modelBuilder.Entity<StockItem>().Property(i => i.Id).ValueGeneratedOnAdd();
            // Default the Quantity for Stock Item to 0
            modelBuilder.Entity<StockItem>().Property(i => i.Quantity).HasDefaultValue(0);
        }
    }
}