namespace CNTO.Launcher
{
    /// <summary>
    /// Represents an arma 3 mod.
    /// </summary>
    public class Mod : IMod
    {
        private string _name;
        private Repository _repository;

        /// <summary>
        /// Initializes a mod from repository.
        /// </summary>
        /// <param name="repository">Repository in which mod belongs.</param>
        /// <param name="name">Mod name.</param>
        internal Mod(Repository repository, string name)
        {
            _repository = repository;
            _name = name;
        }

        /// <summary>
        /// Mod name.
        /// </summary>
        public string Name => _name;

        /// <summary>
        /// Returns mod full path.
        /// </summary>
        /// <returns>Full path of a mod.</returns>
        public string GetFullName()
        {
            return $"{_repository.Path}\\{_name}";
        }

        /// <summary>
        /// Returns if mods have the same name.
        /// </summary>
        /// <param name="mod">Mod to compare to.</param>
        /// <returns>True if mods have the same name, just different repository or version.</returns>
        public bool IsSame(IMod mod)
        {
            return mod.Name.Equals(_name);
        }
    }
}