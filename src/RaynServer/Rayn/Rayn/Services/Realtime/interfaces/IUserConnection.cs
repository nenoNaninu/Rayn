using System;

namespace Rayn.Services.Realtime.Interfaces
{
    public interface IUserConnection : IDisposable
    {
        void Send(byte[] data);
    }
}