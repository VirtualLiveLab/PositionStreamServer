using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using CommonLibrary;
using StreamServer.Data;

namespace StreamServer
{
    public class PacketSender
    {
        public static async Task Send(User user, List<MinimumAvatarPacket> packets, UdpClient udp)
        {
            await Task.Run(async () =>
            {
                var packetCopy = packets.ToList();
                var selfPosition = user.CurrentPacket.Position;
                packetCopy.Sort((a, b) =>
                {
                    var aSquare = Vector3.Square(a.Position, selfPosition);
                    var bSquare = Vector3.Square(b.Position, selfPosition);
                    var comp = aSquare < bSquare ? -1 : 1;
                    return comp;
                });
                if (packetCopy.Count > 100)
                    packetCopy = packetCopy.GetRange(0, 100);
                var buffs = Utility.PacketsToBuffers(packetCopy);
                var tasks = new List<Task>();
                foreach (var buf in buffs)
                {
                    tasks.Add(udp.SendAsync(buf, buf.Length, user.RemoteEndPoint));
                }

                try
                {
                    await Task.WhenAll(tasks);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            });
        }
    }
}
