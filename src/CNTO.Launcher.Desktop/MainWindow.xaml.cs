using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Serilog;
using UI.Source;
using CNTO.Launcher;
using Microsoft.Extensions.Configuration;
using CNTO.Launcher.Infrastructure;
using CNTO.Launcher.Application;
using CNTO.Launcher.Identity;

namespace UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Repositories _repositories;
        private LauncherService _launcherService;
        private IRepositoryCollection _filesystemRepositoryCollection;
        private IProcessRunner _processRunner;
        private readonly LauncherParameters _launcherParameters;

        public MainWindow(IRepositoryCollection filesystemRepositoryCollection, IProcessRunner processRunner, LauncherParameters parameters, LauncherService launcherService)
        {
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(OnUnhandledException);
            InitializeComponent();

            _repositories = (Repositories)(RepositoriesGrid.Resources["Repo"]);
            _filesystemRepositoryCollection = filesystemRepositoryCollection;
            _processRunner = processRunner;
            _launcherParameters = parameters;
            _launcherService = launcherService;

            _repositories.Load(_filesystemRepositoryCollection);
        }

        private void LaunchButton_Click(object sender, RoutedEventArgs e)
        {
            Log.Information("Launching server.");

            var selectedMods = _repositories.GetSelected().Select(r => r.Identity);
            Log.Information("Selected repositories are {selectedMods}.", selectedMods);

            int headlessClients = _repositories.HeadlessClientNumber;
            Log.Information("Number of headless clients is {headlessClients}.", headlessClients);

            List<Dlc> dlcs = new List<Dlc>();

            if (_repositories.GM)
                dlcs.Add(new Dlc("gm"));

            if (_repositories.VN)
                dlcs.Add(new Dlc("vn"));

            if (_repositories.CSLA)
                dlcs.Add(new Dlc("csla"));

            Task.Run(() => _launcherService.StartServerAsync(selectedMods.Select(s => new RepositoryId(s)), dlcs, headlessClients));
        }
    
        private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Log.Fatal((Exception)e.ExceptionObject, "Undhandled exception in application.");
            Log.Fatal("Runtime terminating {flag}.", e.IsTerminating);
        }    
    }
}
