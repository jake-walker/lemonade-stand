﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

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
                //Database.EnsureDeleted();
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
