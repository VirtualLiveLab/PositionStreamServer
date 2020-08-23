using System;

namespace StreamServer.Model
{
    public class PacketContainer
    {
        private readonly object lockObject = new object();
        private MinimumAvatarPacket? currentPacket;

        public MinimumAvatarPacket? CurrentPacket
        {
            get
            {
                lock(lockObject)
                {
                    return currentPacket;
                }
            }
            set
            {
                lock (lockObject)
                {
                    currentPacket = value;
                }
            }
        }
    }
}
