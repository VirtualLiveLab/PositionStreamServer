using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using CommonLibrary;
using EventServerCore;
using LoopLibrary;

namespace StreamClient
{
    public class OutputLoop : BaseLoop<Unit>
    {
        private readonly UdpClient udp;
        private readonly Vector3 _position;

        public OutputLoop(UdpClient udpClient, int interval, string name, Vector3 position)
            : base(interval, name)
        {
            udp = udpClient;
            _position = position;
        }

        protected override void Start()
        {
            var localEndPoint = udp.Client.LocalEndPoint as IPEndPoint;
            var remoteEndPoint = udp.Client.RemoteEndPoint as IPEndPoint;
            Utility.PrintDbg($"localhost: [{localEndPoint!.Port}] -> {remoteEndPoint!.Address}: [{remoteEndPoint.Port}]", Name);
        }

        protected override async Task Update(int count)
        {
            try
            {
                var tasks = new List<Task>();
                var packet = new MinimumAvatarPacket(Name, _position, 0.0f, new Vector4());
                var buf = Utility.PacketToBuffer(packet);
                tasks.Add(udp.SendAsync(buf, buf.Length));
                await Task.WhenAll(tasks);
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Sender stopped");
                throw;
            }
        }
    }
}
