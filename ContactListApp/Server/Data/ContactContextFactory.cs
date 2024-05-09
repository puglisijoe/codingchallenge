using System;
using System.Reflection;
using ContactListApp.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace ContactListApp.Server.Data {
    public class ContactContextFactory : IDesignTimeDbContextFactory<ContactContext> {
        public ContactContext CreateDbContext(string[] args) {
            string environmentName = Environment.GetEnvironmentVariable("Hosting:Environment")
                                     ?? "Development";
            string basePath = AppContext.BaseDirectory;
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{environmentName}.json", true)
                .AddEnvironmentVariables();

            IConfigurationRoot config = builder.Build();
            string connstr = config.GetConnectionString(ContactContext.ContactListDb);
            DbContextOptionsBuilder<ContactContext> optionsBuilder = new DbContextOptionsBuilder<ContactContext>();

            optionsBuilder.UseSqlServer(connstr, builder =>
                builder.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name));
            return new ContactContext(optionsBuilder.Options);
        }
    }
}