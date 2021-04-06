using System;
using Rayn.Services.Realtime.Interfaces;

namespace Rayn.Services.Realtime
{
    public class HttpPollingUserConnectionCreator : IPollingUserConnectionCreator
    {
        private readonly IPollingUserConnectionStore _pollingUserConnectionStore;

        public HttpPollingUserConnectionCreator(IPollingUserConnectionStore pollingUserConnectionStore)
        {
            _pollingUserConnectionStore = pollingUserConnectionStore;
        }

        public IPollingUserConnection Create(Guid threadId, Guid ownerId)
        {
            var connection = _pollingUserConnectionStore.FetchPollingUserConnection(ownerId);
            if (connection != null)
            {
                return connection;
            }

            var newConnection = new HttpPollingUserConnection();
            _pollingUserConnectionStore.Add(ownerId, newConnection);
            return newConnection;
        }
    }
}