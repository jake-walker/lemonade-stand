using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.IO;

namespace Lemonade_Stand
{
    public class DataManager : DbContext
    {
        private static bool _created = false;

        public DataManager()
        {
            if (!_created)
            {
                _created = true;
                Database.EnsureCreated();
            }
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
            modelBuilder.Entity<StockItem>().Property(i => i.Id).HasDefaultValue(Guid.NewGuid().ToString());
            modelBuilder.Entity<StockItem>().Property(i => i.Quantity).HasDefaultValue(0);

            modelBuilder.Entity<Transaction>().Property(t => t.Id).HasDefaultValue(Guid.NewGuid().ToString());
        }

        /// <summary>
        /// A database set for storing stock information.
        /// </summary>
        public DbSet<StockItem> Stock { get; set; }

        /// <summary>
        /// A database set for storing transaction information.
        /// </summary>
        public DbSet<Transaction> Transactions { get; set; }
    }
}
