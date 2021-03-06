using System.Collections.Concurrent;
using StreamServer.Data;

namespace StreamServer
{
    public class ModelManager
    {
        private ModelManager(){}
        private static ModelManager? _instance;
        public static ModelManager Instance => _instance ??= new ModelManager();
        
        public readonly ConcurrentDictionary<long, User> Users = new ConcurrentDictionary<long, User>();
    }
}
