using System.Collections.Generic;
using CNTO.Launcher.Identity;

namespace CNTO.Launcher
{
    public record StartServerCommand(IEnumerable<RepositoryId> SelectedRepositories, IEnumerable<Dlc> Dlcs, int NumberOfClients)
    {
    }
}