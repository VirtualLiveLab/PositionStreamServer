using System;
using System.Net.Sockets;
using System.Threading;
using DebugPrintLibrary;

namespace StreamServer
{
    class Entry
    {
        static void Main(string[] args)
        {
            UdpClient udpClient = new UdpClient(5577);
            var input = new InputLoop(udpClient, 2, 1);
            var output = new OutputLoop(udpClient, 33, 2);
            var statusCheck = new StatusCheckLoop(1000, 3);
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
                    Printer.PrintDbg("Application gracefully shutdown. Bye!");
                    Thread.Sleep(100);
                    break;
                }
            }
        }
    }
}
