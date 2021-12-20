using System;
using CNTO.Launcher;
using CNTO.Launcher.Application;
using CNTO.Launcher.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace UI
{
    public static class Bootstrap
    {
        public static IServiceProvider ServiceProvider { get; private set; }
        public static IConfiguration Configuration { get; private set; }

        public static void Initialize()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File("launcher-log.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            ConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddJsonFile(@"appsettings.json");
            Configuration = builder.Build();

            LauncherParameters launcherParameters = Configuration.Get<LauncherParameters>();

            Log.Information("Settings file read.");
            Log.Information("{@Parameters}", launcherParameters);

            IServiceCollection serviceCollection = new ServiceCollection();

            // register dependencies
            serviceCollection.AddSingleton<IConfiguration>(Configuration);
            serviceCollection.AddTransient<MainWindow>();
            
            serviceCollection.AddSingleton<IRepositoryCollection>(sp =>
            {
                var filesystemRepositoryCollection = new FilesystemRepositoryCollection(launcherParameters.Repositories);
                filesystemRepositoryCollection.Load();
                return filesystemRepositoryCollection;
            });

            serviceCollection.AddTransient<IProcessRunner, WindowsProcessRunner>();
            serviceCollection.AddSingleton<LauncherParameters>(launcherParameters);
            serviceCollection.AddTransient<IExecutionContextStore, JsonExecutionStore>();
            serviceCollection.AddSingleton<LauncherService>();
            serviceCollection.AddTransient<AutoRestartService>();
            serviceCollection.AddLogging(configure => configure.AddSerilog(Log.Logger));

            ServiceProvider = serviceCollection.BuildServiceProvider();
        }
    }
}