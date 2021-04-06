using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using DebugPrintLibrary;
using EventServerCore;
using LoopLibrary;

namespace StreamServer
{
    public class InputLoop : BaseLoop<Unit>
    {
        private readonly UdpClient udp;

        public InputLoop(UdpClient udpClient, int interval, ulong id)
            : base(interval, id)
        {
            udp = udpClient;
        }

        protected override void Start()
        {
            IPEndPoint localEndPoint = (IPEndPoint)udp.Client.LocalEndPoint;
            Printer.PrintDbg($"Any -> localhost: [{localEndPoint?.Port}]");
        }

        protected override async Task Update(int count)
        {
            var tasks = new List<Task>();
            while (udp.Available > 0)
            {
                try
                {
                    UdpReceiveResult res = await udp.ReceiveAsync();
                    tasks.Add(Task.Run(() => PacketProcessor.Process(res)));
                }
                catch (SocketException e)
                {
                    if (e.ErrorCode != 10054) // Client Disconnected.
                        Printer.PrintDbg(e, id);
                }
            }
            await Task.WhenAll(tasks);
        }
    }
}
