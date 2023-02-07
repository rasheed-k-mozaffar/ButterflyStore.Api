global using ButterflyStore.Shared;
global using ButterflyStore.Client.Services.Contracts;
global using ButterflyStore.Client.Services.AppServices;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ButterflyStore.Client;
using Blazored.LocalStorage;
using ButterflyStore.Client.AuthConfiguration;
using Microsoft.AspNetCore.Components.Authorization;



var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");

//Register the Authorization Message Handler so we can access it for out Http Client.
builder.Services.AddTransient<AuthorizationMessageHandler>();

builder.Services.AddHttpClient("ButterflyStore.Api", client =>
{
    client.BaseAddress = new Uri("https://localhost:7195");
}).AddHttpMessageHandler<AuthorizationMessageHandler>();

//Use the IHTTP CLIENT FACTORY to create our http client.
builder.Services.AddScoped(sp => sp.GetService<IHttpClientFactory>()!.CreateClient("ButterflyStore.Api"));

builder.Services.AddAuthorizationCore();

builder.RootComponents.Add<HeadOutlet>("head::after");
//Register our custom auth state provider.
builder.Services.AddScoped<AuthenticationStateProvider, JwtAuthenticationStateProvider>();

//Register Blazored Local Storage so we can access the browser's local storage.
builder.Services.AddBlazoredLocalStorage();

//Register the Auth Service from the CLIENT SERVICES PROJECT to use for registeration and login
builder.Services.AddTransient<IAuthService, AuthService>();


await builder.Build().RunAsync();
