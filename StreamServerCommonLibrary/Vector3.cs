namespace CommonLibrary
{
    public readonly struct Vector3
    {
        public readonly float X;
        public readonly float Y;
        public readonly float Z;

        public Vector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static float Square(Vector3 lhs, Vector3 rhs)
        {
            var diffX = lhs.X - rhs.X;
            var diffY = lhs.Y - rhs.Y;
            var diffZ = lhs.Z - rhs.Z;

            return diffX * diffX + diffY * diffY + diffZ * diffZ;
        }
    }
}
