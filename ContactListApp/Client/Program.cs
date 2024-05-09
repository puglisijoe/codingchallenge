using System;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using ContactListApp.BaseRepository;
using ContactListApp.Client.Data;
using ContactListApp.Controls;
using ContactListApp.Controls.Grid;
using ContactListApp.Model;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace ContactListApp.Client {
    public class Program {
        public const string BaseClient = "ContactListApp.ServerAPI";

        public static async Task Main(string[] args) {
            WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>(nameof(App).ToLowerInvariant());

            builder.Services.AddHttpClient(BaseClient,
                    client =>
                        client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
                .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

            builder.Services.AddTransient(sp =>
                sp.GetRequiredService<IHttpClientFactory>()
                    .CreateClient(BaseClient));

            builder.Services.AddApiAuthorization();
            builder.Services.AddScoped<IBasicRepository<Contact>, WasmRepository>();
            builder.Services.AddScoped<IUnitOfWork<Contact>, WasmUnitOfWork>();
            builder.Services.AddScoped<IPageHelper, PageHelper>();
            builder.Services.AddScoped<IContactFilters, ContactFilters>();
            builder.Services.AddScoped<GridQueryAdapter>();
            builder.Services.AddScoped<EditService>();

            builder.Services.AddScoped(sp =>
                new ClaimsPrincipal(new ClaimsIdentity()));

            await builder.Build().RunAsync();
        }
    }
}