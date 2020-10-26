using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using CommonLibrary;
using EventServerCore;
using LoopLibrary;
using StreamServer.Data;

namespace StreamServer
{
    /**
     * UDP packet receiving class.
     * Do not make multiple instance of this class,
     * because that will break synchronization between
     * Socket.Available() and Socket.ReadAsync().
     * This class will be refactored to singleton.
     */
    public class InputLoop : BaseLoop<Unit>
    {
        private readonly UdpClient udp;

        public InputLoop(UdpClient udpClient, int interval, string name = "Input")
            : base(interval, name)
        {
            udp = udpClient;
        }
        
        protected override void Start()
        {
            IPEndPoint localEndPoint = (IPEndPoint)udp.Client.LocalEndPoint;
            Utility.PrintDbg($"Any -> localhost: [{localEndPoint?.Port}]");
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
                        Utility.PrintDbg(e, Name);
                }
            }
            await Task.WhenAll(tasks);
        }
    }
}
