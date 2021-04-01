﻿namespace Rayn.Models.ApiResponse
{
    public enum StreamerConnectionRequestStatus
    {
        Ok,
        ThreadRoomNotExist,
        BadRequest
    }

    public class StreamerConnectionResponse
    {
        public StreamerConnectionRequestStatus RequestStatus { get; set; }
        public string RealtimeThreadRoomUrl { get; set; }

        public StreamerConnectionResponse(StreamerConnectionRequestStatus requestStatus, string realtimeThreadRoomUrl)
        {
            RequestStatus = requestStatus;
            RealtimeThreadRoomUrl = realtimeThreadRoomUrl;
        }
    }
}