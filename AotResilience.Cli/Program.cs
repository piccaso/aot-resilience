using AotResilience;

using var cts = new CancellationTokenSource();
cts.CancelAfter(10000);

var resilience = new Resilience(-1, ct: cts.Token);
var result = await resilience.Tryhard(() => Task.Run(async () =>
{
    await Task.Delay(500);
    var rng = Random.Shared.Next(0, 100);
    Console.WriteLine(rng);
    if (rng > 1) throw new Exception($"{rng}");
    return rng;
}));

Console.WriteLine($"result = {result}");