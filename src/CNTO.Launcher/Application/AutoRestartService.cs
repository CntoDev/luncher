using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace CNTO.Launcher.Application
{
    public class AutoRestartService
    {
        private readonly LauncherService _launcherService;
        private readonly IExecutionContextStore _executionContextStore;
        private readonly ILogger<AutoRestartService> _logger;

        public AutoRestartService(LauncherService launcherService,
                                  IExecutionContextStore executionContextStore,
                                  ILogger<AutoRestartService> logger)
        {
            _launcherService = launcherService;
            _executionContextStore = executionContextStore;
            _logger = logger;
        }

        public async Task RestartAsync()
        {
            _logger.LogWarning("Restarting server...");
            StartServerCommand command = _executionContextStore.GetLastRunningCommand();
            await _launcherService.StartServerAsync(command.SelectedRepositories, command.Dlcs, command.NumberOfClients);
        }
    }
}