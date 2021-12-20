using CNTO.Launcher.Identity;

namespace CNTO.Launcher
{
    public static class RepositoryFactory
    {
        public static Repository Build (RepositoryId repositoryId, string path, int priority, bool serverSide = false)
        {
            if (serverSide)
                return new ServerRepository(repositoryId, path, priority);

            return new ClientRepository(repositoryId, path, priority);
        }
    }
}