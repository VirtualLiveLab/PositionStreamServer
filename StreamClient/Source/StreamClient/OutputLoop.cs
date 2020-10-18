using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using CommonLibrary;

namespace StreamClient
{
    public class OutputLoop
    {
        public readonly CancellationTokenSource cts = new CancellationTokenSource();
        private readonly UdpClient udp;
        private readonly int interval;
        private readonly string name;
        private readonly Vector3 _position;

        public OutputLoop(UdpClient udpClient, int interval, string name, Vector3 position)
        {
            udp = udpClient;
            this.interval = interval;
            this.name = name;
            _position = position;
        }
        
        public void Start()
        {
            var localEndPoint = udp.Client.LocalEndPoint as IPEndPoint;
            var remoteEndPoint = udp.Client.RemoteEndPoint as IPEndPoint;
            Utility.PrintDbg($"localhost: [{localEndPoint!.Port}] -> {remoteEndPoint!.Address}: [{remoteEndPoint.Port}]", name);
            Task.Run(() => Loop(cts.Token), cts.Token);
        }

        private async Task Loop(CancellationToken token)
        {
            try
            {
                while (true)
                {
                    var tasks = new List<Task>();
                    var delay = Task.Delay(interval, token);
                    tasks.Add(delay);
                    var packet = new MinimumAvatarPacket(name, _position, 0.0f, new Vector4());
                    var buf = Utility.PacketToBuffer(packet);
                    token.ThrowIfCancellationRequested();
                    tasks.Add(udp.SendAsync(buf, buf.Length));
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
