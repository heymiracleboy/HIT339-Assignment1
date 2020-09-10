using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Ass1.Areas.Identity.Data;

    public class ItemDBContext : DbContext
    {
    // Database context for every other model
        public ItemDBContext (DbContextOptions<ItemDBContext> options)
            : base(options)
        {
        }

        public DbSet<Ass1.Models.Item> Item { get; set; }
        public DbSet<Ass1.Models.Sales> Sales { get; set; }
        public DbSet<Ass1.Models.Cart> Carts { get; set; }
}
