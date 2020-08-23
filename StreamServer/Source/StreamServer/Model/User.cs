using System;
using System.Net;

namespace StreamServer.Model
{
    public class User
    {
        public readonly string UserId;
        public readonly PacketContainer PacketContainer = new PacketContainer();
        public IPEndPoint? remoteEndPoint;
        public DateTime? lastUpdated;

        public User(string userId)
        {
            UserId = userId;
        }
    }
}
