using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CNTO.Launcher.Identity;
using Microsoft.Extensions.Logging;

namespace CNTO.Launcher.Application
{
    /// <summary>
    /// Launcher application service, a facade class.
    /// </summary>
    public class LauncherService
    {
        private const int AnarchysDelay = 3000;
        private readonly LauncherParameters _launcherParameters;
        private readonly IRepositoryCollection _repositoryCollection;
        private readonly IProcessRunner _processRunner;
        private readonly IExecutionContextStore _executionContextStore;
        private readonly ILogger<LauncherService> _logger;

        /// <summary>
        /// Initializes a launcher service.
        /// </summary>
        /// <param name="launcherParameters">Launcher parameters, read from JSON.</param>
        /// <param name="repositoryCollection">A collection of repositories.</param>
        /// <param name="processRunner">System process runner, launches the arma server.</param>
        public LauncherService(
            LauncherParameters launcherParameters,
            IRepositoryCollection repositoryCollection,
            IProcessRunner processRunner,
            IExecutionContextStore executionContextStore,
            ILogger<LauncherService> logger
        )
        {
            _launcherParameters = launcherParameters;
            _repositoryCollection = repositoryCollection;
            _processRunner = processRunner;
            _executionContextStore = executionContextStore;
            _logger = logger;
        }

        /// <summary>
        /// Starts the server with selected repositories.
        /// </summary>
        /// <param name="selectedRepositories">Repositories to start the server with.</param>
        public async Task StartServerAsync(IEnumerable<RepositoryId> selectedRepositories, IEnumerable<Dlc> dlcs, int numberOfClients = 0)
        {            
            // store selected options
            StartServerCommand startServerCommand = new StartServerCommand(selectedRepositories, dlcs, numberOfClients);
            _executionContextStore.Store(startServerCommand);

            _logger.LogInformation("Staring server with parameters {@parameters}.", startServerCommand);
            IEnumerable<Repository> repositoryMetadata = _repositoryCollection.WithId(selectedRepositories);

            var server = new ServerBuilder(_launcherParameters.GamePath)
                .WithServerDefaults()
                .WithProfile(_launcherParameters.ProfilePath)
                .WithName("server")
                .WithServerConfig($@"{_launcherParameters.ConfigDirectory}\server.cfg")
                .WithBasicConfig($@"{_launcherParameters.ConfigDirectory}\basic.cfg")
                .WithRepositoryCollection(repositoryMetadata)
                .WithDlcCollection(dlcs)
                .Build();

            _processRunner.Kill(_launcherParameters.GamePath);
            await server.RunAsync(_processRunner);

            for (int i = 0; i < numberOfClients; i++)
            {
                var hc = new ServerBuilder(_launcherParameters.GamePath)
                    .WithHeadlessClientDefaults()
                    .WithProfile(_launcherParameters.ProfilePath)
                    .WithPassword(_launcherParameters.ServerPassword)
                    .WithRepositoryCollection(repositoryMetadata.Where(r => !r.ServerSide))
                    .WithDlcCollection(dlcs)
                    .Build();

                await hc.RunAsync(_processRunner);
            }
        }
    }
}
