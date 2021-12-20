using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using CNTO.Launcher;
using CNTO.Launcher.Application;
using CNTO.Launcher.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        void App_Startup(object sender, StartupEventArgs e)
        {
            try
            {
                Bootstrap.Initialize();

                // Application is running
                // Process command line args

                // Autorestart?
                bool autorestart = e.Args.Contains("-r");

                if (autorestart)
                {
                    Log.Information("Restarting services...");
                    AutoRestartService autoRestartService = Bootstrap.ServiceProvider.GetRequiredService<AutoRestartService>();
                    var task = Task.Run(() => autoRestartService.RestartAsync());
                    task.Wait();
                    Shutdown();
                }
                else
                {
                    MainWindow mainWindow = Bootstrap.ServiceProvider.GetRequiredService<MainWindow>();
                    mainWindow.Show();
                }
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Fatal error while starting application.");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
