using System.Collections.Concurrent;
using System.Collections.Generic;
using Rayn.Services.Realtime.Interfaces;
using Rayn.Services.Realtime.Models;

namespace Rayn.Services.Realtime
{
    public class ConnectionGroupCache : IConnectionGroupCache
    {
        private readonly ConcurrentDictionary<string, Group> _connectionIdToGroupIdDictionary = new();

        public void Add(string connectionId, Group group)
        {
            _connectionIdToGroupIdDictionary.AddOrUpdate(connectionId, group, (key, oldValue) => group);
        }

        public void Remove(string connectionId)
        {
            _connectionIdToGroupIdDictionary.Remove(connectionId, out var _);
        }

        public Group? FindGroup(string connectionId)
        {
            return _connectionIdToGroupIdDictionary.GetValueOrDefault(connectionId);
        }
    }
}