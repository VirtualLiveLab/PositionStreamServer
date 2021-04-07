using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Linq;

namespace CommonLibrary
{
    public class Utility
    {
        public static string BufferToString(byte[] buf)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            for (int i = 0; i < buf.Length; ++i)
            {
                sb.Append(buf[i]);
                sb.Append(", ");
            }

            sb.Remove(sb.Length - 2, 2);
            sb.Append("}");
            return sb.ToString();
        }


        public static MinimumAvatarPacket BufferToPacket(byte[] buf)
        {
            if (buf == null || buf.Length != 31) return null;
            var offset = sizeof(float);
            var userId = BitConverter.ToUInt64(buf, offset);
            var x = BitConverter.ToInt16(buf, offset += sizeof(long));
            var y = BitConverter.ToInt16(buf, offset += sizeof(short));
            var z = BitConverter.ToInt16(buf, offset += sizeof(short));
            offset += sizeof(short);
            var radY = PacketUtil.ConvertInt(buf[offset]);
            var qx = PacketUtil.ConvertInt(buf[offset + 1]);
            var qy = PacketUtil.ConvertInt(buf[offset + 2]);
            var qz = PacketUtil.ConvertInt(buf[offset + 3]);
            var qw = PacketUtil.ConvertInt(buf[offset + 4]);
            var time = BitConverter.ToDouble(buf, offset + 5);
            MinimumAvatarPacket packet =
                new MinimumAvatarPacket(userId, new Vector3(x, y, z), radY,
                    new Vector4(qx, qy, qz, qw), time);
            if (packet.CheckRange())
                return packet;
            else
                return null;
        }

        public static List<MinimumAvatarPacket> BufferToPackets(byte[] buf)
        {
            if (buf != null && buf.Length > 0)
            {
                int begin = sizeof(float);
                var numPackets = BitConverter.ToInt32(buf, 0);
                var supposedBufSize = numPackets * 27 + begin;
                if (buf.Length == supposedBufSize)
                {
                    List<MinimumAvatarPacket> packets = new List<MinimumAvatarPacket>();
                    for (int i = 0; i < numPackets; ++i)
                    {
                        var offset = begin + i * 27;
                        var userId = BitConverter.ToUInt64(buf, offset);
                        var x = BitConverter.ToInt16(buf, offset += sizeof(long));
                        var y = BitConverter.ToInt16(buf, offset += sizeof(short));
                        var z = BitConverter.ToInt16(buf, offset += sizeof(short));
                        offset += sizeof(short);
                        var radY = PacketUtil.ConvertInt(buf[offset]);
                        var qx = PacketUtil.ConvertInt(buf[offset + 1]);
                        var qy = PacketUtil.ConvertInt(buf[offset + 2]);
                        var qz = PacketUtil.ConvertInt(buf[offset + 3]);
                        var qw = PacketUtil.ConvertInt(buf[offset + 4]);
                        var time = BitConverter.ToDouble(buf, offset + 5);
                        MinimumAvatarPacket packet = new MinimumAvatarPacket(userId, new Vector3(x, y, z),
                            radY,
                            new Vector4(qx, qy, qz, qw), time);
                        if (packet.CheckRange()) packets.Add(packet);
                    }

                    return packets;
                }
            }


            return null;
        }

        public static byte[] PacketToBuffer(in MinimumAvatarPacket packet)
        {
            int size = 31;
            byte[] buff = new byte[size];

            unsafe
            {
                fixed (byte* _buff = new byte[size])
                {
                    *(int*)&_buff[0] = 1;
                    *(ulong*)&_buff[4] = packet.PaketId;
                    *(short*)&_buff[12] = packet.Position.x;
                    *(short*)&_buff[14] = packet.Position.y;
                    *(short*)&_buff[16] = packet.Position.z;
                    *(byte*)&_buff[18] = (byte)packet.RadY;
                    *(byte*)&_buff[19] = (byte)packet.NeckRotation.x;
                    *(byte*)&_buff[20] = (byte)packet.NeckRotation.y;
                    *(byte*)&_buff[21] = (byte)packet.NeckRotation.z;
                    *(byte*)&_buff[22] = (byte)packet.NeckRotation.w;
                    *(double*)&_buff[23] = packet.time;
                    Marshal.Copy((IntPtr)_buff, buff, 0, buff.Length);
                }
            }
            return buff;
        }

        public static byte[] PacketsToBuffer(ref List<MinimumAvatarPacket> packets)
        {
            int size = 27 * packets.Count + 4;
            byte[] buff = new byte[size];

            unsafe
            {
                fixed (byte* _buff = new byte[size])
                {
                    *(int*)&_buff[0] = packets.Count;

                    for (int i = 0; i < packets.Count; ++i)
                    {
                        var packet = packets[i];
                        byte* first = &_buff[4 + i * 27];
                        *(ulong*)&first[0] = packet.PaketId;
                        *(short*)&first[8] = packet.Position.x;
                        *(short*)&first[10] = packet.Position.y;
                        *(short*)&first[12] = packet.Position.z;
                        *(byte*)&first[14] = (byte)packet.RadY;
                        *(byte*)&first[15] = (byte)packet.NeckRotation.x;
                        *(byte*)&first[16] = (byte)packet.NeckRotation.y;
                        *(byte*)&first[17] = (byte)packet.NeckRotation.z;
                        *(byte*)&first[18] = (byte)packet.NeckRotation.w;
                        *(double*)&first[19] = packet.time;
                    }
                    Marshal.Copy((IntPtr)_buff, buff, 0, buff.Length);
                }
            }

            return buff;
        }

        public static IEnumerable<byte[]> PacketsToBuffers(ref List<MinimumAvatarPacket> packets)
        {
            var packetsList = new List<List<MinimumAvatarPacket>>();
            const int nSize = 29;
            for (int i = 0; i < packets.Count; i += nSize)
            {
                packetsList.Add(packets.GetRange(i, Math.Min(nSize, packets.Count - i)));
            }
            return packetsList.Select(pcs => PacketsToBuffer(ref pcs));
        }
    }
}