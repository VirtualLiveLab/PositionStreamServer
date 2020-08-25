using System.Collections.Generic;
using StreamServer.Model;
using Xunit;

namespace StreamServer.Test
{
    public class PacketTest
    {
        [Fact]
        public void PacketTest1()
        {
            var packet = new MinimumAvatarPacket("packet1", new Vector3(2.2f, 5.2f, 6.8f), 50,
                new Vector4(2.1f, 4.6f, 3.0f, 3.1f));
            var buff = Utility.PacketsToBuffer(new List<MinimumAvatarPacket> {packet});
            var decodedPacket = Utility.BufferToPackets(buff);
            Assert.Equal(packet.PaketId, decodedPacket![0].PaketId);
            Assert.Equal(packet.Position, decodedPacket![0].Position);
            Assert.Equal(packet.RadY, decodedPacket![0].RadY);
            Assert.Equal(packet.NeckRotation, decodedPacket![0].NeckRotation);
        }
        
        [Fact]
        public void PacketTest2()
        {
            var packet1 = new MinimumAvatarPacket("packet1", new Vector3(2.2f, 5.2f, 6.8f), 50,
                new Vector4(2.1f, 4.6f, 3.0f, 3.1f));
            var packet2 = new MinimumAvatarPacket("packet2", new Vector3(7.5f, 2.6f, 9.2f), 99,
                new Vector4(1.4f, 9.8f, 6.4f, -1.4f));
            var buff = Utility.PacketsToBuffer(new List<MinimumAvatarPacket> {packet1, packet2});
            var decodedPacket = Utility.BufferToPackets(buff);
            
            Assert.Equal(packet1.PaketId, decodedPacket![0].PaketId);
            Assert.Equal(packet1.Position, decodedPacket![0].Position);
            Assert.Equal(packet1.RadY, decodedPacket![0].RadY);
            Assert.Equal(packet1.NeckRotation, decodedPacket![0].NeckRotation);
            
            Assert.Equal(packet2.PaketId, decodedPacket![1].PaketId);
            Assert.Equal(packet2.Position, decodedPacket![1].Position);
            Assert.Equal(packet2.RadY, decodedPacket![1].RadY);
            Assert.Equal(packet2.NeckRotation, decodedPacket![1].NeckRotation);
        }
        
        [Fact]
        public void PacketTest3()
        {
            var packet = new MinimumAvatarPacket("", new Vector3(), 0,
                new Vector4());
            var buff = Utility.PacketsToBuffer(new List<MinimumAvatarPacket> {packet});
            var decodedPacket = Utility.BufferToPackets(buff);
            Assert.Equal(packet.PaketId, decodedPacket![0].PaketId);
            Assert.Equal(packet.Position, decodedPacket![0].Position);
            Assert.Equal(packet.RadY, decodedPacket![0].RadY);
            Assert.Equal(packet.NeckRotation, decodedPacket![0].NeckRotation);
        }

        [Fact]
        public static void PacketTest4()
        {
            var packets = new List<MinimumAvatarPacket>();
            for (int i = 0; i < 100; ++i)
            {
                var packet = new MinimumAvatarPacket($"packet{i}", new Vector3(2.2f, 5.2f, 6.8f), 50,
                    new Vector4(2.1f, 4.6f, 3.0f, 3.1f));
                packets.Add(packet);
            }

            var buffs = Utility.PacketsToBuffers(packets);
            const int nSize = 29;
            for (int i = 0; i < buffs.Count; ++i)
            {
                var decodedPacket = Utility.BufferToPackets(buffs[i]);
                for (int j = 0; j < decodedPacket!.Count; ++j)
                {
                    Assert.Equal(packets[j + i*nSize].Position, decodedPacket![j].Position);
                    Assert.Equal(packets[j + i*nSize].RadY, decodedPacket![j].RadY);
                    Assert.Equal(packets[j + i*nSize].NeckRotation, decodedPacket![j].NeckRotation);
                }
            }
        }
    }
}
