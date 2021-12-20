namespace CNTO.Launcher
{
    public interface IMod
    {
        string Name { get; }
        bool IsSame(IMod mod);
        string GetFullName();
    }
}