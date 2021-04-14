using System.Text.Json;

namespace Rayn.Services.Realtime.Models
{
    public class ThreadMessage
    {
        public string Message { get; set; }

        public ThreadMessage()
        {
        }

        public ThreadMessage(string message)
        {
            Message = message;
        }
    }
}