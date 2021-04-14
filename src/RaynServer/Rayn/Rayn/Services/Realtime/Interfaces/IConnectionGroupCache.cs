using Rayn.Services.Realtime.Models;

namespace Rayn.Services.Realtime.Interfaces
{
    public interface IConnectionGroupCache
    {
        void Add(string connectionId, Group groupName);
        void Remove(string connectionId);
        Group FindGroup(string connectionId);
    }
}