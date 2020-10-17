using System;
using System.Net.Sockets;

namespace StreamClient
{
    public class Entry
    {
        static void Main(string[] args)
        {
            UdpClient udpClient = new UdpClient(5577);
            var input = new InputLoop(udpClient, 2);
            //var output = new OutputLoop(udpClient, 33);
            input.Start();
            //output.Start();
            while (true)
            {
                var line = Console.ReadLine();
                if (line == "exit()")
                {
                    input.cts.Cancel();
                    //output.cts.Cancel();
                    Console.WriteLine("Application gracefully shutdown. Bye!");
                    break;
                }
            }
        }
    }
}