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
            var pollingConnection = new HttpPollingUserConnection();
            _pollingUserConnectionStore.Add(ownerId, pollingConnection);
            return pollingConnection;
        }
    }
}