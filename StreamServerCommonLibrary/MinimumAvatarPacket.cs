using System;

namespace CommonLibrary
{
    /// <summary>
    /// パケットのモデルクラス
    /// Warning : インスタンス作成時に必ずCheckRange関数でにて確認する必要がある
    /// </summary>
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
        /// range is Abs.127
        /// </summary>
        public readonly int RadY;

        /// <summary>
        /// Rotation 4bit
        /// </summary>
        public readonly Vector4 NeckRotation;

        /// <summary>
        /// time 8bit
        /// </summary>
        public readonly double time;

        public MinimumAvatarPacket(ulong paketId, Vector3 position, int radY, Vector4 neckRotation, double time)
        {
            PaketId = paketId;
            Position = position;
            RadY = radY;
            NeckRotation = neckRotation;
            this.time = time;
        }

        
        /// <summary>
        /// Rangeが決まっているものについてチェックする
        /// </summary>
        /// <returns></returns>
        public bool CheckRange()
        {
            var radY = this.RadY;
            var neckX = this.NeckRotation.x;
            var neckY = this.NeckRotation.y;
            var neckZ = this.NeckRotation.z;
            var neckW = this.NeckRotation.w;
            return Math.Abs(radY) < 128 && Math.Abs(neckX) < 128 && Math.Abs(neckY) < 128 && Math.Abs(neckZ) < 128 && Math.Abs(neckW) < 128;
        }
    }
}