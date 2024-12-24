using System;
using System.Threading;
using System.Threading.Tasks;

namespace AotResilience
{
    public class Resilience
    {
        private readonly int _limit;
        private readonly TimeSpan? _delay;
        private readonly int _delayAfter;
        private readonly CancellationToken _ct;
        private int _failCount;
        public Action<int, Exception>? ExceptionAction = null;

        public Resilience(int limit = 9, TimeSpan? delay = null, int delayAfter = 2, CancellationToken ct = default)
        {
            if (limit < 0) limit = int.MaxValue;

            _limit = limit;
            _delay = delay;
            _delayAfter = delayAfter;
            _ct = ct;
        }

        public async Task<T> Tryhard<T>(Func<Task<T>> fn)
        {
            while (!_ct.IsCancellationRequested)
            {
                try
                {
                    var task = fn();
                    return await task;
                }
                catch (Exception ex)
                {
                    _failCount++;
                    ExceptionAction?.Invoke(_failCount, ex);
                    if (_failCount >= _limit) throw;
                }

                if (_delay.HasValue && _failCount >= _delayAfter) await Task.Delay(_delay.Value, _ct);
            }

            throw new OperationCanceledException(_ct);
        }
    }
}
