using System;
using System.Net.Sockets;
using StreamServer.Data;

namespace StreamServer
{
    class Entry
    {
        static void Main(string[] args)
        {
            ModelManager.Instance.Users["kai101"] = new User("kai101");
            ModelManager.Instance.Users["user2"] = new User("user2");
            ModelManager.Instance.Users["user3"] = new User("user3");
            UdpClient udpClient = new UdpClient(5577);
            var input = new InputLoop(udpClient, 2);
            var output = new OutputLoop(udpClient, 33);
            input.Start();
            output.Start();
            while (true)
            {
                var line = Console.ReadLine();
                if (line == "exit()")
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
