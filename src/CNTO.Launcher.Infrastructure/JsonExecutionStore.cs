using System.IO;
using Newtonsoft.Json;

namespace CNTO.Launcher.Infrastructure
{
    public class JsonExecutionStore : IExecutionContextStore
    {
        private const string _fileName = "ExecutionStore.json";

        public StartServerCommand GetLastRunningCommand()
        {
            string json = File.ReadAllText(_fileName);
            return JsonConvert.DeserializeObject<StartServerCommand>(json);
        }

        public void Store(StartServerCommand startServerCommand)
        {
            string json = JsonConvert.SerializeObject(startServerCommand);
            File.WriteAllText(_fileName, json);
        }
    }
}
