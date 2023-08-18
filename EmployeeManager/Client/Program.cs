global using EmployeeManager.Shared.Models;
global using EmployeeManager.Shared.Dtos;
global using EmployeeManager.Shared.JsonObjects;
global using EmployeeManager.Client.Services.EmployeeService;
using EmployeeManager.Client;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<IEmployeeService, EmployeeService>();

await builder.Build().RunAsync();
