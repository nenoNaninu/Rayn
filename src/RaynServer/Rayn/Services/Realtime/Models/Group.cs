using System;

namespace Rayn.Services.Realtime.Models
{
    public class Group
    {
        public readonly string GroupName;
        public readonly Guid ThreadId;

        public Group(string groupName, Guid threadId)
        {
            GroupName = groupName;
            ThreadId = threadId;
        }
    }
}
