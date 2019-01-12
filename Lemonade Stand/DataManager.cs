using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.IO;

namespace Lemonade_Stand
{
    public sealed class DataManager : DbContext
    {
        private static bool _created = false;

        public DataManager()
        {
            if (_created) return;
            _created = true;
            Database.EnsureCreated();
        }

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

            modelBuilder.Entity<User>().HasData(new User()
            {
                Id = 1,
                Administrator = true,
                Password = "Password"
            });
            modelBuilder.Entity<User>().Property(u => u.Id).ValueGeneratedOnAdd();

            modelBuilder.Entity<Product>().Property(p => p.Id).ValueGeneratedOnAdd();
        }

        

        /// <summary>
        /// A database set for storing stock information.
        /// </summary>
        public Microsoft.EntityFrameworkCore.DbSet<StockItem> Stock { get; set; }

        /// <summary>
        /// A database set for storing transaction information.
        /// </summary>
        public Microsoft.EntityFrameworkCore.DbSet<Transaction> Transactions { get; set; }

        /// <summary>
        /// A database set for storing worker information.
        /// </summary>
        public Microsoft.EntityFrameworkCore.DbSet<User> Users { get; set; }

        /// <summary>
        /// A database set for storing products.
        /// </summary>
        public Microsoft.EntityFrameworkCore.DbSet<Product> Products { get; set; }
    }
}
