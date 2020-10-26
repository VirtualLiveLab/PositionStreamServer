using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using CommonLibrary;
using StreamServer.Data;

namespace StreamServer
{
    class Entry
    {
        static void Main(string[] args)
        {
            UdpClient udpClient = new UdpClient(5577);
            var input = new InputLoop(udpClient, 2);
            var output = new OutputLoop(udpClient, 33);
            var statusCheck = new StatusCheckLoop(1000);
            input.Run();
            output.Run();
            statusCheck.Run();
            while (true)
            {
                Thread.Sleep(100);
                var line = Console.ReadLine();
                if (line == "exit!")
                {
                    input.Cts.Cancel();
                    output.Cts.Cancel();
                    Console.WriteLine("Application gracefully shutdown. Bye!");
                    Thread.Sleep(100);
                    break;
                }
            }
        }
    }
}
