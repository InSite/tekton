﻿using Tek.Terminal;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Serilog;

// Step 1. Load configuration settings (from appsettings.json) before doing anything else.

var configuration = BuildConfiguration();

var settings = GetSettings(configuration);

// Step 2. Configure logging before we build the application host to ensure we capture all log
// entries, including those generated during the host initialization process. This is critical for
// diagnosing startup issues, monitoring initialization steps, and providing consistent, centralized
// logging throughout the application lifecycle.

Log.Logger = ConfigureLogging(settings.Kernel.Telemetry.Logging.Path);

// Step 3. Build the application host with all services registered in the DI container.

var host = BuildHost(settings);

// Step 4. Start up the application.

await Startup(host);

// Step 5. Shut down the application

await Shutdown(host);


// -------------------------------------------------------------------------------------------------


IConfigurationRoot BuildConfiguration()
{
    var basePath = AppContext.BaseDirectory;

    var builder = new ConfigurationBuilder()
        .AddEnvironmentVariables()
        .SetBasePath(basePath)
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

    AddLocalSettings(builder);

    return builder
        .Build();

    void AddLocalSettings(IConfigurationBuilder builder)
    {
        if (AddLocalFile(builder, basePath))
            return;

        if (AddLocalFile(builder, Path.Combine(basePath, "..")))
            return;

        if (AddLocalFile(builder, Path.Combine(basePath, "..", "..")))
            return;

        AddLocalFile(builder, Path.Combine(basePath, "..", "..", ".."));
    }

    bool AddLocalFile(IConfigurationBuilder builder, string folder)
    {
        var file = Path.Combine(folder, "appsettings.local.json");
     
        if (!File.Exists(file))
            return false;

        builder = builder.AddJsonFile(file);

        return true;
    }
}

TektonSettings GetSettings(IConfigurationRoot configuration)
{
    var section = configuration.GetRequiredSection("Tekton");
    var settings = section.Get<TektonSettings>()!;
    settings.Kernel.Release.Directory = AppContext.BaseDirectory;
    return settings;
}

Serilog.ILogger ConfigureLogging(string path)
{
    return new LoggerConfiguration()
        .MinimumLevel.Debug()
        .WriteTo.Console()
        .WriteTo.File(path, rollingInterval: RollingInterval.Day)
        .CreateLogger();
}

IHost BuildHost(TektonSettings settings)
{
    var builder = Host.CreateDefaultBuilder(args)

        .ConfigureServices((context, services) =>
        {
            services.AddSingleton(settings);
            services.AddSingleton(settings.Kernel.Release);
            services.AddSingleton(settings.Metadata.Database.Connection);
            services.AddSingleton(settings.Plugin.Integration);
            services.AddSingleton(settings.Plugin.Integration.AstronomyApi);
            services.AddSingleton(settings.Plugin.Integration.VisualCrossing);

            services.AddLogging(builder =>
            {
                builder.ClearProviders();
                builder.AddSerilog(dispose: true);
            });

            services.AddMonitoringServices(settings.Kernel.Telemetry.Monitoring);

            services.AddSingleton<ILog, Tek.Service.Log>();
            services.AddSingleton<IMonitor, Tek.Service.Monitor>();
            services.AddSingleton<IJsonSerializer, JsonSerializer>();
            services.AddSingleton<DatabaseCommander>();

            services.AddTransient<Application>();

            services.AddSingleton<Spectre.Console.Cli.ITypeRegistrar>(new TypeRegistrar(services));
        });

    var host = builder.Build();

    return host;
}

async Task Startup(IHost host)
{
    var monitor = host.Services.GetRequiredService<IMonitor>();

    monitor.Information("Starting up.");

    var app = host.Services.GetRequiredService<Application>();

    await app.RunAsync(args);
}

async Task Shutdown(IHost host)
{
    var monitor = host.Services.GetRequiredService<IMonitor>();

    monitor.Information("Shutting down.");

    await monitor.FlushAsync();
}