using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using CommonLibrary;

namespace StreamClient
{
    public class Entry
    {
        static void Main(string[] args)
        {
            var str = args.Length > 0 ? args[0] : Console.ReadLine();
            var ipaddr = args.Length > 1 ? args[1] : "127.0.0.1";
            Console.WriteLine(ipaddr);
            var k = Int32.Parse(str);
            var ctsList = new List<CancellationTokenSource>();
            for (int i = k; i < 1000 + k; ++i)
            {
                UdpClient udpClient = UdpClientFactory.CreateClient(ipaddr , 5577);
                var position = new Vector3(i%33*4, 0.5f, i/33*2);
                var output = new OutputLoop(udpClient, 16, $"user{i}", position);
                output.Run();
                ctsList.Add(output.Cts);
            }
            while (true)
            {
                Thread.Sleep(100);
                var line = Console.ReadLine();
                if (line == "exit!")
                {
                    foreach (var cts in ctsList)
                    {
                        cts.Cancel();
                    }
                    Console.WriteLine("Application gracefully shutdown. Bye!");
                    Thread.Sleep(100);
                    break;
                }
            }
        }
    }
}
