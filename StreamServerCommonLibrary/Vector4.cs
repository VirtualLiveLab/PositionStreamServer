namespace CommonLibrary
{
    public readonly struct Vector4
    {
        public readonly int x;
        public readonly int y;
        public readonly int z;
        public readonly int w;

        public Vector4(int x, int y, int z, int w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }
    }
}
