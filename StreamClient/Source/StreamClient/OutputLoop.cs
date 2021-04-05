using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using CommonLibrary;
using CommonLibrary.Exception;
using DebugPrintLibrary;
using EventServerCore;
using LoopLibrary;

namespace StreamClient
{
    public class OutputLoop : BaseLoop<Unit>
    {
        private readonly UdpClient udp;
        private readonly Vector3 _position;

        public OutputLoop(UdpClient udpClient, int interval, ulong id, Vector3 position)
            : base(interval, id)
        {
            udp = udpClient;
            _position = position;
        }

        protected override void Start()
        {
            var localEndPoint = udp.Client.LocalEndPoint as IPEndPoint;
            var remoteEndPoint = udp.Client.RemoteEndPoint as IPEndPoint;
            Printer.PrintDbg($"localhost: [{localEndPoint!.Port.ToString()}] -> {remoteEndPoint!.Address}: [{remoteEndPoint.Port.ToString()}]", id);
        }

        protected override async Task Update(int count)
        {
            try
            {
                var tasks = new List<Task>();
                var packet = new MinimumAvatarPacket(id, _position, (0 + count) % 127, new Vector4(), 0);
                if (!packet.CheckRange())
                {
                    throw new MinimumAvatarPacketCreativeException("the value must be less than or equal to 127 in absolute value.");
                }
                var buf = Utility.PacketToBuffer(packet);
                tasks.Add(udp.SendAsync(buf, buf.Length));
                await Task.WhenAll(tasks);
            }
            catch (OperationCanceledException)
            {
                Printer.PrintDbg("Sender stopped");
                throw;
            }
        }
    }
}
