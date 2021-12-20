using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace CNTO.Launcher
{
    public class Server : IServer
    {
        private const int ServerStartDelay = 3000;
        private readonly string _processPath;
        private IDictionary<string, string> _arguments;

        internal Server(string processPath)
        {
            _processPath = processPath;
            _arguments = new Dictionary<string, string>();
        }

        public async Task RunAsync(IProcessRunner processRunner)
        {
            StringBuilder builder = new StringBuilder();

            foreach (var arg in _arguments)
            {
                builder.Append($"-{arg.Key}");

                if (!string.IsNullOrWhiteSpace(arg.Value))
                    builder.Append($"={arg.Value}");

                builder.Append(" ");
            }

            string arguments = builder.ToString().TrimEnd();
            processRunner.Run(_processPath, arguments);
            await Task.Delay(ServerStartDelay);
        }

        internal void WithArgument(string option, string value = "")
        {
            _arguments[option] = value;
        }
    }
}