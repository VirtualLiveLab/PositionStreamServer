using System.Net;
using System.Net.Sockets;

namespace StreamClient
{
    public static class UdpClientFactory
    {
        public static UdpClient CreateClient(string serverIpAddress, int serverPort)
        {
            var remoteEndpoint = new IPEndPoint(IPAddress.Parse(serverIpAddress), serverPort);
            var udpClient = new UdpClient();
            try
            {
                //--Automatically bind to an available port by binding to port 0.
                udpClient.Client.Bind(new IPEndPoint(IPAddress.Parse("0.0.0.0"), 0));
            }
            catch (SocketException)
            {
                //--Binding to port 0 throws SocketException.
                //Debug.Log(e);
            }
            udpClient.Connect(remoteEndpoint);
            return udpClient;
        }
    }
}
