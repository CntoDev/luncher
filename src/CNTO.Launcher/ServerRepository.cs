using System.Collections.Generic;
using CNTO.Launcher.Identity;

namespace CNTO.Launcher
{
    internal class ServerRepository : Repository
    {
        internal ServerRepository(RepositoryId repositoryId, string path, int priority) : base(repositoryId, path, priority) { }

        public override bool ServerSide => true;

        internal override void AppendToModset(ModSet modSet)
        {
            foreach (var mod in Mods)
            {
                modSet.AddServerSideMod(mod);
            }
        }
    }
}