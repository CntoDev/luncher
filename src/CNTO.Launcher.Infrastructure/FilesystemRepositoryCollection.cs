using System.Collections.Generic;
using System.IO;
using System.Linq;
using CNTO.Launcher.Identity;
using Serilog;

namespace CNTO.Launcher.Infrastructure
{
    public class FilesystemRepositoryCollection : IRepositoryCollection
    {
        private readonly IEnumerable<RepositoryParameters> _parameters;
        private readonly List<Repository> _repositories;

        public FilesystemRepositoryCollection(IEnumerable<RepositoryParameters> parameters)
        {
            _parameters = parameters;
            _repositories = new List<Repository>();
        }

        public void Load()
        {
            Log.Information("Loading repository collection.");

            foreach (var par in _parameters)
            {
                Log.Information("Loading repository, parameters {@par}", par);
                Repository repository = RepositoryFactory.Build(new RepositoryId(par.Id), par.Path, par.Priority, par.ServerSide);
                var directories = Directory.GetDirectories(par.Path);
                var directoryNames = directories.Select(d => Path.GetFileName(d)).ToArray();
                Log.Information("Mods present in the repository are: {mods}", directoryNames);
                repository.LoadMods(directoryNames);
                _repositories.Add(repository);
            }
        }

        public IEnumerable<Repository> All()
        {
            return _repositories;
        }

        public IEnumerable<Repository> WithId(IEnumerable<RepositoryId> selectedRepositories)
        {
            return _repositories.Where(r => selectedRepositories.Contains(r.RepositoryId));
        }
    }
}
