using System;
using System.Collections.Generic;
using CNTO.Launcher.Identity;
using CNTO.Launcher.Application;
using NUnit.Framework;
using Moq;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace CNTO.Launcher.Test
{
    public class LauncherTests
    {
        private Repository _mainRepo;
        private Repository _devRepo;
        private LauncherParameters _launcherParameters;
        private Mock<IRepositoryCollection> _collection;
        private Mock<IProcessRunner> _processRunnerMock;
        private Mock<IExecutionContextStore> _executionContextStoreMock;
        private Mock<ILogger<LauncherService>> _loggerMock;

        [SetUp]
        public void Setup()
        {
            _mainRepo = new ClientRepository(new RepositoryId("main"), @"c:\cnto\main", 1);
            _mainRepo.HasMod("@mod1");
            _mainRepo.HasMod("@mod2");
            _mainRepo.HasMod("@mod4");

            _devRepo = new ClientRepository(new RepositoryId("dev"), @"c:\cnto\dev", 2);
            _devRepo.HasMod("@mod1");
            _devRepo.HasMod("@mod3");

            _launcherParameters = new LauncherParameters()
            {
                ConfigDirectory = @"C:\cnto\dev\arma-startup-scripts\configDir",
                GamePath = @"C:\Program Files (x86)\Steam\steamapps\common\Arma 3\arma3server_x64.exe",
                ProfilePath = @"C:\cnto\dev\arma-startup-scripts\configDir\profiles"
            };

            _collection = new Mock<IRepositoryCollection>();
            _collection.Setup(m => m.All()).Returns(new List<Repository>() { _mainRepo, _devRepo });
            _collection.Setup(m => m.WithId(It.IsAny<IEnumerable<RepositoryId>>())).Returns(new List<Repository>() { _mainRepo, _devRepo });

            _processRunnerMock = new Mock<IProcessRunner>();
        }

        [Test]
        public void RunBothRepos()
        {
            string expectedArguments = "-port=2302 -noSplash -noLand -enableHT -hugePages -filePatching -profiles=C:\\cnto\\dev\\arma-startup-scripts\\configDir\\profiles -name=server -config=C:\\cnto\\dev\\arma-startup-scripts\\configDir\\server.cfg -cfg=C:\\cnto\\dev\\arma-startup-scripts\\configDir\\basic.cfg -mod=c:\\cnto\\main\\@mod2;c:\\cnto\\main\\@mod4;c:\\cnto\\dev\\@mod1;c:\\cnto\\dev\\@mod3";
            _processRunnerMock.Setup(p => p.Run("C:\\Program Files (x86)\\Steam\\steamapps\\common\\Arma 3\\arma3server_x64.exe", expectedArguments))
                .Verifiable();

            _executionContextStoreMock.Setup(s => s.Store(It.IsAny<StartServerCommand>())).Verifiable();

            CNTO.Launcher.Application.LauncherService launcher = new Application.LauncherService(
                _launcherParameters,
                _collection.Object,
                _processRunnerMock.Object,
                _executionContextStoreMock.Object,
                _loggerMock.Object
            );

            Task.Run(() => launcher.StartServerAsync(new List<RepositoryId>() { new RepositoryId("main"), new RepositoryId("dev") }, Enumerable.Empty<Dlc>())).Wait();
            _processRunnerMock.Verify();
            _executionContextStoreMock.Verify();
        }

        [Test]
        public void RunMain()
        {
            string expectedArguments = "-port=2302 -noSplash -noLand -enableHT -hugePages -filePatching -profiles=C:\\cnto\\dev\\arma-startup-scripts\\configDir\\profiles -name=server -config=C:\\cnto\\dev\\arma-startup-scripts\\configDir\\server.cfg -cfg=C:\\cnto\\dev\\arma-startup-scripts\\configDir\\basic.cfg -mod=c:\\cnto\\main\\@mod1;c:\\cnto\\main\\@mod2;c:\\cnto\\main\\@mod4";
            _processRunnerMock.Setup(p => p.Run("C:\\Program Files (x86)\\Steam\\steamapps\\common\\Arma 3\\arma3server_x64.exe", expectedArguments))
                .Verifiable();

            _executionContextStoreMock.Setup(s => s.Store(It.IsAny<StartServerCommand>())).Verifiable();

            _collection.Setup(m => m.WithId(It.IsAny<IEnumerable<RepositoryId>>())).Returns((IEnumerable<RepositoryId> ids) =>
            {
                var all = new List<Repository>() { _mainRepo, _devRepo };

                return all.Where(m => ids.Contains(m.RepositoryId));
            });

            CNTO.Launcher.Application.LauncherService launcher = new Application.LauncherService(
                _launcherParameters,
                _collection.Object,
                _processRunnerMock.Object,
                _executionContextStoreMock.Object,
                _loggerMock.Object
            );

            Task.Run(() => launcher.StartServerAsync(new List<RepositoryId>() { new RepositoryId("main") }, Enumerable.Empty<Dlc>())).Wait();
            _processRunnerMock.Verify();
            _executionContextStoreMock.Verify();
        }
    }
}