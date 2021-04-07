using System.Collections.Generic;
using System.Linq;
using CommonLibrary;
using Xunit;
using Xunit.Abstractions;

namespace StreamServer.Test
{
    public class PacketTest
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public PacketTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void PacketTest1()
        {
            var packet = new MinimumAvatarPacket(1, new Vector3(2, 5, 6), 10,
                new Vector4(2, 4, 3, 120), 0);
            Assert.True(packet.CheckRange(), "packet.CheckRange()");
            var packets = new List<MinimumAvatarPacket> {packet};
            var buff = Utility.PacketsToBuffer(ref packets);
            var decodedPacket = Utility.BufferToPackets(buff);
            Assert.Equal(packet.PaketId, decodedPacket![0].PaketId);
            Assert.Equal(packet.Position, decodedPacket![0].Position);
            Assert.Equal(packet.RadY, decodedPacket![0].RadY);
            Assert.Equal(packet.NeckRotation, decodedPacket![0].NeckRotation);
        }

        [Fact]
        public void PacketTest2()
        {
            var packet1 = new MinimumAvatarPacket(1, new Vector3((short) 2.2f, (short) 5.2f, (short) 6.8f), 50,
                new Vector4(2, 4, 3, 3), 0);
            var packet2 = new MinimumAvatarPacket(2, new Vector3((short) 7.5f, (short) 2.6f, (short) 9.2f), 99,
                new Vector4(1, 9, 6, -1), 0);
            Assert.True(packet1.CheckRange() && packet2.CheckRange(), "packet1.CheckRange() && packet2.CheckRange()");
            var packets = new List<MinimumAvatarPacket> {packet1, packet2};
            var buff = Utility.PacketsToBuffer(ref packets);
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
            var packet = new MinimumAvatarPacket(3, new Vector3(), 0,
                new Vector4(), 0);
            Assert.True(packet.CheckRange(), "packet.CheckRange()");
            var packets = new List<MinimumAvatarPacket> {packet};
            var buff = Utility.PacketsToBuffer(ref packets);
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
                var packet = new MinimumAvatarPacket((ulong) i, new Vector3((short) 2.2f, (short) 5.2f, (short) 6.8f),
                    50,
                    new Vector4(2, 4, 3, 3), 0);
                packets.Add(packet);
                Assert.True(packet.CheckRange(), $"packet.CheckRange({i})");
            }

            var buffs = Utility.PacketsToBuffers(ref packets).ToList();

            const int nSize = 29;
            for (int i = 0; i < buffs.Count; ++i)
            {
                var decodedPacket = Utility.BufferToPackets(buffs[i]);
                for (int j = 0; j < decodedPacket!.Count; ++j)
                {
                    Assert.Equal(packets[j + i * nSize].Position, decodedPacket![j].Position);
                    Assert.Equal(packets[j + i * nSize].RadY, decodedPacket![j].RadY);
                    Assert.Equal(packets[j + i * nSize].NeckRotation, decodedPacket![j].NeckRotation);
                }
            }
        }
    }
}