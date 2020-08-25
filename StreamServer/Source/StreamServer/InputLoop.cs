﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using StreamServer.Model;

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
            PrintDbg($"Any -> localhost: [{localEndPoint?.Port}]");
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
                            var res = await udp.ReceiveAsync();
                            var buf = res.Buffer;
                            var packets = Utility.BufferToPackets(buf);
                            if (packets != null && ModelManager.Instance.Users.ContainsKey(packets[0].PaketId))
                            {
                                var packet = packets[0];
                                var user = ModelManager.Instance.Users[packet.PaketId];
                                if (user.CurrentPacket == null)
                                {
                                    PrintDbg($"Connected: [{user.UserId}] " +
                                             $"({res.RemoteEndPoint.Address}: {res.RemoteEndPoint.Port})");
                                    user.RemoteEndPoint = res.RemoteEndPoint;
                                }

                                user.CurrentPacket = packet;
                                user.DateTimeBox = new DateTimeBox(DateTime.Now);
                            }
                        }
                        catch (SocketException e)
                        {
                            if (e.ErrorCode != 10054) //Client Disconnected.
                                PrintDbg(e);
                        }
                    }

                    token.ThrowIfCancellationRequested();
                    await delay;
                }
            }
            catch (OperationCanceledException)
            {
                PrintDbg("Receiver stopped");
                throw;
            }
            catch (Exception e)
            {
                PrintDbg(e);
                throw;
            }
        }
        
        private void PrintDbg<T>(T obj)
        {
            Console.WriteLine($"[{name}] {obj}");
        }
    }
}