using System.Threading.Tasks;

namespace ScreenOverwriter
{
    public interface IServer
    {
        Task<IMessageReceiver<string>> ConnectAsync();
        Task<bool> CloseAsync();
    }
}