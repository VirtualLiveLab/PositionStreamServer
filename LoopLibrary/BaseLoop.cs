using System;
using System.Threading;
using System.Threading.Tasks;
using DebugPrintLibrary;
using EventServerCore;

namespace LoopLibrary
{
    public abstract class BaseLoop<T>
    {
        public readonly string Name;
        private readonly int _interval;
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        private int _count;
        public CancellationTokenSource Cts => _cts;

        protected BaseLoop(int interval, string name = "Input")
        {
            _interval = interval;
            Name = name;
        }

        public Task<Result<T>> Run()
        {
            Start();
            return Task.Run(() => Loop(_cts.Token), _cts.Token);
        }

        private async Task<Result<T>> Loop(CancellationToken token)
        {
            try
            {
                while (true)
                {
                    var delay = Task.Delay(_interval, token);

                    await Update(_count);
                    _count++;
                    
                    await delay;
                }
            }
            catch (OperationCanceledException)
            {
                OnCancel();
                return new Result<T>(false);
            }
            catch (OperationCompletedException<Result<T>> e)
            {
                Printer.PrintDbg("Loop completed");
                return e.Result;
            }
            catch (Exception e)
            {
                Printer.PrintDbg($"[{Name}] Exception on loop\n" + e);
                return new Result<T>(false);
            }
        }

        protected virtual void Start(){}

        protected abstract Task Update(int count);

        protected virtual void OnCancel()
        {
            Printer.PrintDbg($"[{Name}] Loop canceled");
        }

        public void Done(T result)
        {
            throw new OperationCompletedException<Result<T>>(new Result<T>(true ,result));
        }

        public void Cancel()
        {
            throw new OperationCanceledException();
        }
    }
}
