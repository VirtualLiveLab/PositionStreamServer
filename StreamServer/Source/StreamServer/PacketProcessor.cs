using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using CommonLibrary;
using DebugPrintLibrary;
using StreamServer.Data;

namespace StreamServer
{
    public static class PacketProcessor
    {
        public static async Task Process(UdpReceiveResult res)
        {
            var packet = Utility.BufferToPacket(res.Buffer);
            if (!ValidatePacket(packet))
            {
                return;
            }
            var users = ModelManager.Instance.Users;

            User? user;
            if (!users.TryGetValue(packet.PaketId, out user))
            {
                // RemoteEndPointから来た最初のパケットの場合
                user = users[packet.PaketId] = new User(packet.PaketId);
                user.RemoteEndPoint = res.RemoteEndPoint;
                Printer.PrintDbg($"Connected: [{user.UserId.ToString()}] " +
                                 $"({res.RemoteEndPoint.Address}: {res.RemoteEndPoint.Port.ToString()})");
            }

            user.CurrentPacket = packet;
            user.DateTimeBox = new DateTimeBox(DateTime.Now);
            user.IsConnected = true;
        }

        private static bool ValidatePacket(in MinimumAvatarPacket? packet)
        {
            return packet != null;
        }
    }
}