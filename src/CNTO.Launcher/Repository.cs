using System;
using System.Collections.Generic;
using CNTO.Launcher.Identity;

namespace CNTO.Launcher
{
    public abstract class Repository
    {
        private List<Mod> _mods;
        private string _path;

        internal Repository(RepositoryId repositoryId, string path, int priority)
        {
            RepositoryId = repositoryId;
            _path = path;
            Priority = priority;

            _mods = new List<Mod>();
        }

        public RepositoryId RepositoryId { get; private set; }
        public string Path => _path;
        public int Priority { get; private set; }
        public abstract bool ServerSide { get; }

        public void HasMod(string modName)
        {
            Mod mod = new Mod(this, modName);
            _mods.Add(mod);
        }

        public void LoadMods(string[] mods)
        {
            foreach (string modName in mods)
            {
                HasMod(modName);
            }
        }

        internal abstract void AppendToModset(ModSet modSet);

        protected IEnumerable<Mod> Mods => _mods;
    }
}