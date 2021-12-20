using System.Collections.Generic;
using CNTO.Launcher.Identity;

namespace CNTO.Launcher
{
    public interface IRepositoryCollection
    {
        IEnumerable<Repository> All();
        IEnumerable<Repository> WithId(IEnumerable<RepositoryId> selectedRepositories);
    }
}