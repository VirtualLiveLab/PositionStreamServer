namespace StreamServer.Model
{
    public class PositionPacket
    {
        public readonly string PaketId;
        public readonly Vector3 Position;

        public PositionPacket(string paketId, Vector3 position)
        {
            PaketId = paketId;
            Position = position;
        }
    }
}
