using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

[assembly: InternalsVisibleTo("CNTO.Launcher.Test")]
namespace CNTO.Launcher
{    
    internal class ModSet
    {
        private List<IMod> _mods = new List<IMod>();
        private List<IMod> _serverMods = new List<IMod>();

        public bool Empty => !_mods.Any() && !_serverMods.Any();

        public ModSet Append(Repository repository)
        {
            repository.AppendToModset(this);

            return this;
        }

        public Dictionary<string, string> ExtractArguments()
        {
            Dictionary<string, string> arguments = new Dictionary<string, string>();

            if (_mods.Any())
                arguments["mod"] = string.Join(";", _mods.Select(x => x.GetFullName()));

            if (_serverMods.Any())
                arguments["serverMod"] = string.Join(";", _serverMods.Select(x => x.GetFullName()));
            
            return arguments;            
        }

        internal void AddMod(Mod mod) => AddMod(_mods, mod);

        internal void AddServerSideMod(Mod mod) => AddMod(_serverMods, mod);

        private void AddMod(List<IMod> modList, Mod mod)
        {
            var existingMod = modList.FirstOrDefault(m => m.Name == mod.Name);

            if (existingMod != null)
                modList.Remove(existingMod);
            
            modList.Add(mod);            
        }
    }
}