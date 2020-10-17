using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
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
    public class InputLoop
    {
        public readonly CancellationTokenSource cts = new CancellationTokenSource();
        private readonly UdpClient udp;
        private readonly int interval;
        private readonly string name;

        public InputLoop(UdpClient udpClient, int interval, string name = "Input")
        {
            udp = udpClient;
            this.interval = interval;
            this.name = name;
        }
        
        public void Start()
        {
            IPEndPoint localEndPoint = (IPEndPoint)udp.Client.LocalEndPoint;
            Utility.PrintDbg($"Any -> localhost: [{localEndPoint?.Port}]");
            Task.Run(() => Loop(cts.Token), cts.Token);
        }

        private async Task Loop(CancellationToken token)
        {
            try
            {
                while (true)
                {
                    var delay = Task.Delay(interval, token);
                    while (udp.Available > 0)
                    {
                        try
                        {
                            UdpReceiveResult res;
                            res = await udp.ReceiveAsync();
                            ProcessPacket.Process(res);
                        } catch (SocketException e)
                        {
                            if (e.ErrorCode != 10054) //Client Disconnected.
                                Utility.PrintDbg(e);
                        }
                    }
                    token.ThrowIfCancellationRequested();
                    await delay;
                }
            }
            catch (OperationCanceledException)
            {
                Utility.PrintDbg("Receiver stopped");
                throw;
            }
            catch (Exception e)
            {
                Utility.PrintDbg(e);
                throw;
            }
        }
    }
}
