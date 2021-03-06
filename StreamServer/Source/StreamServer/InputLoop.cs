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

        public InputLoop(UdpClient udpClient, int interval, long id)
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
                    UdpReceiveResult res;
                    res = await udp.ReceiveAsync();
                    var process = PacketProcessor.Process(res);
                    tasks.Add(process);
                } catch (SocketException e)
                {
                    if (e.ErrorCode != 10054) //Client Disconnected.
                        Printer.PrintDbg(e, id);
                }
            }
            await Task.WhenAll(tasks);
        }
    }
}
