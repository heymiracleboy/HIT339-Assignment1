using System;
using Ass1.Areas.Identity.Data;
using Ass1.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(Ass1.Areas.Identity.IdentityHostingStartup))]
namespace Ass1.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<Ass1Context>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("Ass1ContextConnection")));

                services.AddDefaultIdentity<Ass1User>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<Ass1Context>();
            });
        }
    }
}