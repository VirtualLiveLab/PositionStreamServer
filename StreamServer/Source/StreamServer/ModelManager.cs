using System.Collections.Concurrent;
using StreamServer.Model;

namespace StreamServer
{
    public class ModelManager
    {
        private ModelManager(){}
        private static ModelManager? _instance;
        public static ModelManager Instance => _instance ??= new ModelManager();
        
        public readonly ConcurrentDictionary<string, User> Users = new ConcurrentDictionary<string, User>();
    }
}
