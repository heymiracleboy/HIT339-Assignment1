using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ass1.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Ass1.Data
{
    //Database context for custom user data
    public class Ass1Context : IdentityDbContext<Ass1User>
    {
        public Ass1Context(DbContextOptions<Ass1Context> options)
            : base(options)
        {
        }

        public DbSet<Ass1User> Ass1Users { get; set; }

     

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
