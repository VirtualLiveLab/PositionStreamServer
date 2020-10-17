using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;
using CommonLibrary;
using StreamServer.Data;

namespace StreamServer
{
    public class PacketSender
    {
        public static async Task Send(List<MinimumAvatarPacket> packets, User user, UdpClient udp)
        {
            await Task.Run(async () =>
            {
                var buffs = Utility.PacketsToBuffers(packets);
                var tasks = new List<Task>();
                foreach (var buf in buffs)
                {
                    tasks.Add(udp.SendAsync(buf, buf.Length, user.RemoteEndPoint));
                }
                await Task.WhenAll(tasks);
            });
        }
    }
}
