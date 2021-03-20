namespace CommonLibrary
{
    public class MinimumAvatarPacket
    {
        
        /// <summary>
        /// Id 8bit
        /// </summary>
        public readonly ulong PaketId;
      
        /// <summary>
        /// Position 6bit
        /// </summary>
        public readonly Vector3 Position;

        /// <summary>
        /// RadY 1bit
        /// </summary>
        public readonly float RadY;
    
        /// <summary>
        /// Rotation 4bit
        /// </summary>
        public readonly Vector4 NeckRotation;
  
        /// <summary>
        /// time 8bit
        /// </summary>
        public readonly double time;

        public MinimumAvatarPacket(ulong paketId, Vector3 position, float radY, Vector4 neckRotation, double time)
        {
            PaketId = paketId;
            Position = position;
            RadY = radY;
            NeckRotation = neckRotation;
            this.time = time;
        }
    }
}
