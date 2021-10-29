using System;

namespace Rayn.Services.Responses
{
    public enum StreamerConnectionRequestStatus
    {
        Ok,
        ThreadRoomNotExist,
        BadRequest
    }

    public class StreamerConnectionResponse
    {
        public StreamerConnectionRequestStatus RequestStatus { get; }
        public string RealtimeThreadRoomUrl { get; }
        public Guid ThreadId { get; }

        public StreamerConnectionResponse(StreamerConnectionRequestStatus requestStatus, string realtimeThreadRoomUrl, Guid threadId)
        {
            RequestStatus = requestStatus;
            RealtimeThreadRoomUrl = realtimeThreadRoomUrl;
            ThreadId = threadId;
        }
    }
}
