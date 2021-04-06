using System;
using System.Threading;
using System.Threading.Tasks;
using DebugPrintLibrary;
using EventServerCore;
using System.Linq;
using System.Collections.Generic;

namespace LoopLibrary
{
    public abstract class BaseLoop<T>
    {
        public readonly ulong id;
        private readonly int _interval;
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        private int _count;
        public CancellationTokenSource Cts => _cts;
        private readonly int _maxTimespans = 100;
        private Queue<double> _timespans = new Queue<double>();

        protected BaseLoop(int interval, ulong id = 1)
        {
            _interval = interval;
            this.id = id;
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

                    var start = DateTime.Now;
                    await Update(_count++);
                    var timespan = DateTime.Now - start;

                    lock (_timespans)
                    {
                        _timespans.Enqueue(timespan.TotalMilliseconds);
                        if (_timespans.Count > _maxTimespans)
                        {
                            _timespans.Dequeue();
                        }
                    }

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
                Printer.PrintDbg($"[{id}] Exception on loop\n" + e);
                return new Result<T>(false);
            }
        }

        protected virtual void Start() { }

        protected abstract Task Update(int count);

        protected virtual void OnCancel()
        {
            Printer.PrintDbg($"[{id}] Loop canceled");
        }

        public void Done(T result)
        {
            throw new OperationCompletedException<Result<T>>(new Result<T>(true, result));
        }

        public void Cancel()
        {
            throw new OperationCanceledException();
        }

        public double GetFps()
        {
            lock (_timespans)
            {
                if (_timespans.Count > 0)
                {
                    return 1000 / _timespans.Average();
                }
                else
                {
                    return 0;
                }
            }
        }
    }
}
