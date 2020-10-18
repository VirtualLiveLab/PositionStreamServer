using System;
using System.Net;
using CommonLibrary;

namespace StreamServer.Data
{
    public class User
    {
        public readonly string UserId;
        public volatile bool IsConnected;
        public volatile MinimumAvatarPacket? CurrentPacket;
        public volatile IPEndPoint? RemoteEndPoint;
        public volatile DateTimeBox? DateTimeBox;

        public User(string userId)
        {
            UserId = userId;
        }

        public User(User instance)
        {
            UserId = instance.UserId;
            IsConnected = instance.IsConnected;
            RemoteEndPoint = instance.RemoteEndPoint;
            DateTimeBox = instance.DateTimeBox;
        }
    }
}
