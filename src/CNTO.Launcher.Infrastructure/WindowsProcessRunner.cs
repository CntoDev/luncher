using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Serilog;

namespace CNTO.Launcher.Infrastructure
{
    public class WindowsProcessRunner : IProcessRunner
    {
        public void Run(string processPath, string arguments)
        {
            Log.Information("Starting process with {process} {arguments}.", processPath, arguments);
            Process process = Process.Start(processPath, arguments);
            Log.Information("Process started.");
        }

        public void Kill(string processPath)
        {
            var processName = Path.GetFileNameWithoutExtension(processPath);
            var processArray = Process.GetProcessesByName(processName);

            foreach (var proc in processArray)
            {
                Log.Information("Killing process {pid}.", proc.Id);
                proc.Kill();
            }
        }
    }
}
