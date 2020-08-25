using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using StreamServer.Model;

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
            PrintDbg($"localhost: [{localEndPoint?.Port}] -> Any");
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
                            if (packet != null && DateTime.Now - user.Value.DateTimeBox.LastUpdated > new TimeSpan(0, 0, 2))
                            {
                                PrintDbg($"Disconnected: [{user.Value.UserId}] " +
                                         $"({user.Value.RemoteEndPoint!.Address}: {user.Value.RemoteEndPoint.Port})");
                                user.Value.CurrentPacket = packet = null;
                            }
                        }
                        if (packet != null)
                        {
                            packets.Add(packet);
                            users.Add(user.Value);
                        }
                    }
                    var ofsettedPackets = new List<MinimumAvatarPacket>();
                    foreach (var packet in packets)
                    {
                        ofsettedPackets.Add(new MinimumAvatarPacket(packet.PaketId,
                            new Vector3(packet.Position.X, packet.Position.Y, packet.Position.Z + 5f),
                            packet.RadY,
                            packet.NeckRotation));
                    }
                    var buf = Utility.PacketsToBuffer(packets);
                    foreach (var user in users)
                    {
                        await udp.SendAsync(buf, buf.Length, user.RemoteEndPoint);
                    }
                    token.ThrowIfCancellationRequested();
                    await delay;
                }
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Sender stopped");
                throw;
            }
        }

        private void PrintDbg<T>(T str)
        {
            Console.WriteLine($"[{name}] {str}");
        }
    }
}
