using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Mausam;
using Mausam.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");


builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<INominatim, Nominatim>();
builder.Services.AddScoped<IOpenMeteo, OpenMeteoService>();
builder.Services.AddScoped<ILocationService, LocationService>();
await builder.Build().RunAsync();