using System.Collections.Concurrent;
using CommonLibrary;

namespace StreamClient
{
    public class ModelManager
    {
        private ModelManager(){}
        private static ModelManager? _instance;
        public static ModelManager Instance => _instance ??= new ModelManager();
        
        public readonly ConcurrentDictionary<string, MinimumAvatarPacket> UserToPacket = new ConcurrentDictionary<string, MinimumAvatarPacket>();
    }
}
