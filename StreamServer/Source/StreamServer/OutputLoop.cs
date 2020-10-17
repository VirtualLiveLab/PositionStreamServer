using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using CommonLibrary;
using StreamServer.Data;

namespace StreamServer
{
    public class OutputLoop
    {
        public readonly CancellationTokenSource cts = new CancellationTokenSource();
        private readonly UdpClient udp;
        private readonly int interval;
        private readonly string name;

        public OutputLoop(UdpClient udpClient, int interval, string name = "Output")
        {
            udp = udpClient;
            this.interval = interval;
            this.name = name;
        }
        
        public void Start()
        {
            var localEndPoint = udp.Client.LocalEndPoint as IPEndPoint;
            Utility.PrintDbg($"localhost: [{localEndPoint?.Port}] -> Any");
            Task.Run(() => Loop(cts.Token), cts.Token);
        }

        private async Task Loop(CancellationToken token)
        {
            try
            {
                while (true)
                {
                    var delay = Task.Delay(interval, token);
                    List<MinimumAvatarPacket> packets = new List<MinimumAvatarPacket>();
                    List<User> users = new List<User>();
                    foreach (var user in ModelManager.Instance.Users)
                    {
                        MinimumAvatarPacket? packet;
                        {
                            packet = user.Value.CurrentPacket;
                            if (user.Value.IsConnected && packet != null && DateTime.Now - user.Value.DateTimeBox!.LastUpdated > new TimeSpan(0, 0, 2))
                            {
                                Utility.PrintDbg($"Disconnected: [{user.Value.UserId}] " +
                                         $"({user.Value.RemoteEndPoint!.Address}: {user.Value.RemoteEndPoint.Port})");
                                user.Value.CurrentPacket = packet = null;
                                user.Value.IsConnected = false;
                            }
                        }
                        if (user.Value.IsConnected) users.Add(user.Value);
                        if (packet != null) packets.Add(packet);
                    }
                    List<Task> tasks = new List<Task>();
                    foreach (var user in users)
                    {
                        tasks.Add(PacketSender.Send(packets, user, udp));
                    }
                    token.ThrowIfCancellationRequested();
                    tasks.Add(delay);
                    await Task.WhenAll(tasks);
                }
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Sender stopped");
                throw;
            }
        }
    }
}
