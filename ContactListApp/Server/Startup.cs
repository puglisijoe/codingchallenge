using ContactListApp.BaseRepository;
using ContactListApp.DataAccess;
using ContactListApp.Model;
using ContactListApp.Repository;
using ContactListApp.Server.Data;
using ContactListApp.Server.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ContactListApp.Server {
    public class Startup {
        private static readonly string DefaultConnection
            = nameof(DefaultConnection);

        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services) {
            services.AddDbContext<ApplicationAuditDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString(DefaultConnection)));

            services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationAuditDbContext>();

            services.AddIdentityServer()
                .AddApiAuthorization<ApplicationUser, ApplicationAuditDbContext>();

            services.AddAuthentication()
                .AddIdentityServerJwt();

            services.AddDbContextFactory<ContactContext>(opt =>
                opt.UseSqlServer(
                        Configuration.GetConnectionString(ContactContext.ContactListDb))
                    .EnableSensitiveDataLogging());

            services.AddScoped<IRepository<Contact, ContactContext>,
                ContactRepository>();

            services.AddScoped<IBasicRepository<Contact>>(sp =>
                sp.GetService<IRepository<Contact, ContactContext>>());
            services.AddScoped<IUnitOfWork<Contact>, UnitOfWork<ContactContext, Contact>>();
            services.AddScoped<ContactGenerator>();

            services.AddControllersWithViews();
            services.AddRazorPages();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseWebAssemblyDebugging();
            }
            else {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseIdentityServer();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}