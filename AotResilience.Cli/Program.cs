using AotResilience;

var cts = new CancellationTokenSource();
cts.CancelAfter(1000);
var resilience = new Resilience(100, TimeSpan.FromMilliseconds(50), 10, cts.Token)
{
    ExceptionAction = (i, exception) => Console.WriteLine($"{i} : {exception.Message}")
};

Task<int> Job()
{
    var rng = Random.Shared.Next(1, 100);
    if (rng > 1) throw new Exception($"{rng}");
    return Task.FromResult(rng);
}

var result = await resilience.Tryhard(Job);
Console.WriteLine($"result = {result}");