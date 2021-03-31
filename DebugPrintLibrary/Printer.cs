using System;

namespace DebugPrintLibrary
{
    public static class Printer
    {
        public static Action<string> OnDebugPrint;
        
        public static void PrintDbg(object str, object sender = null)
        {
#if DEBUG
            var output = $"[{sender}] {str}";
            if (OnDebugPrint != null) OnDebugPrint(output);
            else Console.WriteLine(output);
#endif
        }
    }
}
