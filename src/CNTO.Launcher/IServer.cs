using System.Threading.Tasks;

namespace CNTO.Launcher
{
    public interface IServer
    {
        Task RunAsync(IProcessRunner processRunner);
    }
}