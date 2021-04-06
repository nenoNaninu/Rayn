using Rayn.Services.Realtime.Interfaces;
using RxWebSocket;

namespace Rayn.Services.Realtime
{
    public sealed class WebSocketUserConnection : IUserConnection
    {
        private readonly IWebSocketClient _webSocketClient;

        public WebSocketUserConnection(IWebSocketClient webSocketClient)
        {
            _webSocketClient = webSocketClient;
        }

        public void Dispose()
        {
            _webSocketClient.Dispose();
        }

        public void Send(byte[] data)
        {
            _webSocketClient.Send(data);
        }
    }
}