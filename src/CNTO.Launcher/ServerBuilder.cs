using System;
using System.Collections.Generic;
using System.Linq;

namespace CNTO.Launcher
{
    public class ServerBuilder
    {
        private Server _server;
        private List<Repository> _repositories;
        private List<Dlc> _dlcs;

        public ServerBuilder(string processPath)
        {
            _server = new Server(processPath);
            _repositories = new List<Repository>();
            _dlcs = new List<Dlc>();
        }

        public ServerBuilder WithServerDefaults()
        {
            _server.WithArgument("port", "2302");
            _server.WithArgument("noSplash");
            _server.WithArgument("noLand");
            _server.WithArgument("enableHT");
            _server.WithArgument("hugePages");
            _server.WithArgument("filePatching");

            return this;
        }

        public ServerBuilder WithHeadlessClientDefaults()
        {
            _server.WithArgument("port", "2302");
            _server.WithArgument("noSplash");
            _server.WithArgument("noLand");
            _server.WithArgument("enableHT");
            _server.WithArgument("hugePages");
            _server.WithArgument("client");
            _server.WithArgument("connect", "127.0.0.1");

            return this;
        }

        public ServerBuilder WithProfile(string profilePath)
        {
            _server.WithArgument("profiles", profilePath);
            return this;
        }

        public ServerBuilder WithName(string serverName)
        {
            _server.WithArgument("name", serverName);
            return this;
        }

        public ServerBuilder WithPassword(string password)
        {
            _server.WithArgument("password", password);
            return this;
        }

        public ServerBuilder WithServerConfig(string serverConfigPath)
        {
            _server.WithArgument("config", serverConfigPath);
            return this;
        }

        public ServerBuilder WithBasicConfig(string basicConfigPath)
        {
            _server.WithArgument("cfg", basicConfigPath);
            return this;
        }

        public ServerBuilder WithRepository(Repository repository)
        {
            _repositories.Add(repository);
            return this;
        }

        public ServerBuilder WithDlc(Dlc dlc)
        {
            _dlcs.Add(dlc);
            return this;
        }

        public ServerBuilder WithRepositoryCollection(IEnumerable<Repository> repositoryMetadata)
        {
            foreach (var repo in repositoryMetadata)
                WithRepository(repo);

            return this;
        }

        public ServerBuilder WithDlcCollection(IEnumerable<Dlc> dlcCollection)
        {
            foreach (var dlc in dlcCollection)
                WithDlc(dlc);

            return this;
        }

        public IServer Build()
        {
            Dictionary<string, string> extraArguments = new Dictionary<string, string>();

            if (_repositories.Any())
            {
                var sortedRepositories = _repositories.OrderBy(r => r.Priority);
                ModSet modSet = new ModSet();

                foreach (var repo in sortedRepositories)
                {
                    modSet.Append(repo);
                }

                var modSetArguments = modSet.ExtractArguments();

                foreach (var pair in modSetArguments)
                {
                    extraArguments.Add(pair.Key, pair.Value);
                }
            }

            if (_dlcs.Any())
            {
                extraArguments["beta"] = "creatordlc";
                string existingMods = string.Empty;
                bool hasMods = extraArguments.TryGetValue("mod", out existingMods);
                string dlcs = string.Join(";", _dlcs.Select(x => x.Name));

                if (hasMods)
                    extraArguments["mod"] = $"{existingMods};{dlcs};";
                else
                    extraArguments["mod"] = $"{dlcs};";
            }

            foreach (var argument in extraArguments)
                _server.WithArgument(argument.Key, argument.Value);

            return _server;
        }
    }
}