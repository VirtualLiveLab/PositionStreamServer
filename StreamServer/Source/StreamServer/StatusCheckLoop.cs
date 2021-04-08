using System.Threading.Tasks;
using DebugPrintLibrary;
using EventServerCore;
using LoopLibrary;

namespace StreamServer
{
    public class StatusCheckLoop : BaseLoop<Unit>
    {
        private InputLoop inputLoop;
        private OutputLoop outputLoop;
        public StatusCheckLoop(int interval, ulong id, InputLoop inputLoop, OutputLoop outputLoop)
            : base(interval, id)
        {
            this.inputLoop = inputLoop;
            this.outputLoop = outputLoop;
        }

        protected override async Task Update(int count)
        {
            Printer.PrintDbg($"Num clients: {ModelManager.Instance.Users.Count.ToString()}");
            Printer.PrintDbg($"input : {inputLoop.GetFps(),5:f0} fps");
            Printer.PrintDbg($"output: {outputLoop.GetFps(),5:f0} fps");
        }
    }
}