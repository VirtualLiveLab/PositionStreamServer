using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using StreamServer.Model;

namespace StreamServer
{
    public static class ProcessPacket
    {
        public static async Task Process(UdpReceiveResult res)
        {
            try
            {
                await Task.Run(() =>
                {
                    var buf = res.Buffer;
                    var optPacket = Utility.BufferToPacket(buf);
                    if (!ValidatePacket(optPacket)) return;
                    var packet = optPacket!;
                    var users = ModelManager.Instance.Users;
                    
                    var user = users.ContainsKey(packet.PaketId)
                        ? users[packet.PaketId]
                        : users[packet.PaketId] = new User(packet.PaketId);
                    if (!user.IsConnected)
                    {
                        user.IsConnected = true;
                        Utility.PrintDbg($"Connected: [{user.UserId}] " +
                                         $"({res.RemoteEndPoint.Address}: {res.RemoteEndPoint.Port})");
                    }
                    
                    user.RemoteEndPoint = res.RemoteEndPoint;

                    user.CurrentPacket = packet;
                    user.DateTimeBox = new DateTimeBox(DateTime.Now);
                    users[packet.PaketId] = user;
                });
            }
            catch (Exception e)
            {
                Utility.PrintDbg(e);
            }
        }

        private static bool ValidatePacket(in MinimumAvatarPacket? packet)
        {
            return packet != null;
            //&& ModelManager.Instance.Users.ContainsKey(packet.PaketId);
        }
    }
}