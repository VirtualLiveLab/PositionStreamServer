using System.Threading.Tasks;
using DebugPrintLibrary;
using EventServerCore;
using LoopLibrary;

namespace StreamServer
{
    public class StatusCheckLoop : BaseLoop<Unit>
    {
        public StatusCheckLoop(int interval, ulong id)
            : base(interval, id)
        {
        }

        protected override async Task Update(int count)
        {
            Printer.PrintDbg($"Num clients: {ModelManager.Instance.Users.Count}");
        }
    }
}