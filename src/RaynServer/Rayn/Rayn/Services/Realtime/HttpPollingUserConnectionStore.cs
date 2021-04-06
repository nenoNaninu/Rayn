using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Rayn.Services.Realtime.Interfaces;

namespace Rayn.Services.Realtime
{
    // シングルトン想定
    public sealed class HttpPollingUserConnectionStore : IPollingUserConnectionStore
    {
        private readonly ConcurrentDictionary<Guid, IPollingUserConnection> _connections = new();

        public IPollingUserConnection FetchPollingUserConnection(Guid ownerId)
        {
            return _connections.GetValueOrDefault(ownerId);
        }

        public void Add(Guid ownerId, IPollingUserConnection connection)
        {
            _connections.TryAdd(ownerId, connection);

            connection.OnDispose()
                .Subscribe(_ =>
                {
                    _connections.TryRemove(ownerId, out var _);
                });
        }
    }
}