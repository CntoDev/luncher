using CNTO.Launcher.Identity;

namespace CNTO.Launcher
{
    /// <summary>
    /// Repository that contains mods that need to be placed both on client and server (the "normal" repository).
    /// </summary>
    internal class ClientRepository : Repository
    {
        /// <summary>
        /// Initializes a client repository.
        /// </summary>
        /// <param name="repositoryId">Repository identity.</param>
        /// <param name="path">OS path.</param>
        /// <param name="priority">Priority that influences on ranking mods.</param>
        internal ClientRepository(RepositoryId repositoryId, string path, int priority) : base(repositoryId, path, priority) { }
        
        /// <summary>
        /// Is mod server side only?
        /// </summary>
        public override bool ServerSide => false;

        /// <summary>
        /// Append mods within repository to a modset.
        /// </summary>
        /// <param name="modSet">Mod set to add mods to.</param>
        internal override void AppendToModset(ModSet modSet)
        {
            foreach (var mod in Mods)
            {
                modSet.AddMod(mod);
            }
        }
    }
}