using System.Text.Json;

namespace Rayn.Services.Realtime.Models
{
    public class MessageModel
    {

        public string Message { get; set; }

        /// <summary>
        /// javascript側でkeep aliveの設定が出来る雰囲気が無かったのでとりあえず。
        /// </summary>
        public bool PingPong { get; set; }

        public MessageModel()
        {
        }

        public MessageModel(string message, bool pingPong)
        {
            Message = message;
            PingPong = pingPong;
        }
    }
}