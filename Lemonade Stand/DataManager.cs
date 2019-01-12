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

        protected override void OnConfiguring(DbContextOptionsBuilder optionBuilder)
        {
            var directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "LemonadeStand.sqlite");
            Debug.WriteLine($"Using {directory} for database storage.");
            optionBuilder.UseSqlite($"Data Source={directory}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StockItem>().Property(i => i.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<StockItem>().Property(i => i.Quantity).HasDefaultValue(0);

            modelBuilder.Entity<Transaction>().Property(t => t.Id).ValueGeneratedOnAdd();

            modelBuilder.Entity<User>().HasData(new User
            {
                Id = 1,
                Administrator = true,
                Password = "Password"
            });
            modelBuilder.Entity<User>().Property(u => u.Id).ValueGeneratedOnAdd();

            modelBuilder.Entity<Product>().Property(p => p.Id).ValueGeneratedOnAdd();
        }
    }
}