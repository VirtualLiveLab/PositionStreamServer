using System;
using System.Threading.Tasks;
using EventServerCore;
using LoopLibrary;

namespace StreamServer
{
    public class StatusCheckLoop : BaseLoop<Unit>
    {
        public StatusCheckLoop(int interval, string name = "Input")
            : base(interval, name)
        {
        }

        protected override async Task Update(int count)
        {
            Console.WriteLine($"Num clients: {ModelManager.Instance.Users.Count}");
        }
    }
}