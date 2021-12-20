namespace CNTO.Launcher
{
    public interface IExecutionContextStore
    {
        void Store(StartServerCommand startServerCommand);
        StartServerCommand GetLastRunningCommand();
    }
}