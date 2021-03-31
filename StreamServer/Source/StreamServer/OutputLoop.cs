using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using CommonLibrary;
using DebugPrintLibrary;
using EventServerCore;
using LoopLibrary;
using StreamServer.Data;

namespace StreamServer
{
    public class OutputLoop : BaseLoop<Unit>
    {
        private readonly UdpClient udp;

        public OutputLoop(UdpClient udpClient, int interval, ulong id)
            : base(interval, id)
        {
            udp = udpClient;
        }
        
        protected override void Start()
        {
            var localEndPoint = udp.Client.LocalEndPoint as IPEndPoint;
#if DEBUG
            Printer.PrintDbg($"localhost: [{localEndPoint?.Port.ToString()}] -> Any");
#endif
        }

        protected override async Task Update(int count)
        {
            List<MinimumAvatarPacket> packets = new List<MinimumAvatarPacket>();
            List<User> users = new List<User>();
            foreach (var kvp in ModelManager.Instance.Users)
            {
                if(kvp.Value == null) continue;
                var user = kvp.Value;
                MinimumAvatarPacket? packet = user.CurrentPacket;
                {
                    if (user.IsConnected && packet != null && DateTime.Now - user.DateTimeBox!.LastUpdated > new TimeSpan(0, 0, 1))
                    {
#if DEBUG
                        Printer.PrintDbg($"Disconnected: [{user.UserId.ToString()}] " +
                                 $"({user.RemoteEndPoint!.Address}: {user.RemoteEndPoint.Port.ToString()})");
#endif
                        user.CurrentPacket = packet = null;
                        user.IsConnected = false;
                        ModelManager.Instance.Users.TryRemove(kvp.Key, out var dummy);
                    }
                }
                if (user.IsConnected) users.Add(user);
                if (packet != null) packets.Add(packet);
            }
            List<Task> tasks = new List<Task>();
            foreach (var user in users)
            {
                tasks.Add(PacketSender.Send(user, packets, udp));
            }
            await Task.WhenAll(tasks);
        }
    }
}
