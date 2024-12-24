namespace AotResilience
{
    public class Resilience
    {
        private readonly int _limit;
        private readonly TimeSpan? _delay;
        private readonly int _delayAfter;
        private int _failCount;
        public Action<int, Exception>? ExceptionAction = null;


        public Resilience(int limit = 9, TimeSpan? delay = null, int delayAfter = 2)
        {
            _limit = limit;
            _delay = delay;
            _delayAfter = delayAfter;
        }

        public async Task Tryhard(Func<Task> fn)
        {
            while (true)
            {
                try
                {
                    var task = fn();
                    await task;
                    return;
                }
                catch (Exception ex)
                {
                    _failCount++;
                    ExceptionAction?.Invoke(_failCount, ex);
                    if (_failCount >= _limit) throw;
                }

                if (_delay.HasValue && _failCount >= _delayAfter) await Task.Delay(_delay.Value);
            }
        }

        public async Task<T> Tryhard<T>(Func<Task<T>> fn)
        {
            while (true)
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

                if (_delay.HasValue && _failCount >= _delayAfter) await Task.Delay(_delay.Value);
            }
        }
    }
}
