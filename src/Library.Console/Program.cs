using Microsoft.Extensions.DependencyInjection;
using Library.Infrastructure.Data;
using Library.ApplicationCore;
using Microsoft.Extensions.Configuration;

var services = new ServiceCollection();

var configuration = new ConfigurationBuilder()
.SetBasePath(Directory.GetCurrentDirectory())
.AddJsonFile("appSettings.json")
.Build();

services.AddSingleton<IConfiguration>(configuration);

services.AddScoped<IPatronRepository, JsonPatronRepository>();
services.AddScoped<ILoanRepository, JsonLoanRepository>();
services.AddScoped<ILoanService, LoanService>();
services.AddScoped<IPatronService, PatronService>();

services.AddSingleton<JsonData>();

services.AddSingleton<ConsoleApp>(sp =>
    new ConsoleApp(
        sp.GetRequiredService<ILoanService>(),
        sp.GetRequiredService<IPatronService>(),
        sp.GetRequiredService<IPatronRepository>(),
        sp.GetRequiredService<ILoanRepository>(),
        sp.GetRequiredService<JsonData>()
    )
);

var servicesProvider = services.BuildServiceProvider();

var consoleApp = servicesProvider.GetRequiredService<ConsoleApp>();
consoleApp.Run().Wait();
