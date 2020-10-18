using System;
using System.Collections.Generic;
using System.Net.Sockets;
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
            input.Start();
            output.Start();
            while (true)
            {
                var line = Console.ReadLine();
                if (line == "exit!")
                {
                    input.cts.Cancel();
                    output.cts.Cancel();
                    Console.WriteLine("Application gracefully shutdown. Bye!");
                    break;
                }
            }
        }
    }
}
