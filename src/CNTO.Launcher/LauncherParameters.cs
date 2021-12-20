using System.Collections.Generic;

namespace CNTO.Launcher
{
    public class LauncherParameters
    {
        public string GamePath { get; set; }
        public string ProfilePath { get; set; }
        public string ConfigDirectory { get; set; }
        public string ServerPassword { get; set; }
        public IEnumerable<RepositoryParameters> Repositories { get; set; }
    }
}