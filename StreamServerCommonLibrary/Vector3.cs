using System;
using System.ComponentModel.Design;

namespace CommonLibrary
{
    public readonly struct Vector3
    {
        public readonly short x;
        public readonly short y;
        public readonly short z;
        
        public Vector3(short x, short y, short z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public static float Square(Vector3 lhs, Vector3 rhs)
        {
            var diffX = lhs.x - rhs.x;
            var diffY = lhs.y - rhs.y;
            var diffZ = lhs.z - rhs.z;

            return diffX * diffX + diffY * diffY + diffZ * diffZ;
        }
    }
}
