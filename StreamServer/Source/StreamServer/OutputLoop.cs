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
                    foreach (var kvp in ModelManager.Instance.Users)
                    {
                        if(kvp.Value == null) continue;
                        var user = kvp.Value;
                        MinimumAvatarPacket? packet = user.CurrentPacket;
                        {
                            if (user.IsConnected && packet != null && DateTime.Now - user.DateTimeBox!.LastUpdated > new TimeSpan(0, 0, 2))
                            {
                                Utility.PrintDbg($"Disconnected: [{user.UserId}] " +
                                         $"({user.RemoteEndPoint!.Address}: {user.RemoteEndPoint.Port})");
                                user.CurrentPacket = packet = null;
                                user.IsConnected = false;
                            }
                        }
                        if (user.IsConnected) users.Add(user);
                        if (packet != null) packets.Add(packet);
                    }
                    List<Task> tasks = new List<Task>();
                    foreach (var user in users)
                    {
                        if (packets.Count > 100)
                            packets = packets.GetRange(0, 100);
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
