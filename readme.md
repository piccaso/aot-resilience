# AotResilience
### Simple resilience compatible with AOT compilation
Very simple retry implementation.  
This is not a full featured Library like [Polly](https://www.pollydocs.org/) by design.  
But it is compatible with AOT compilation and legacy projects.  

There is an official way to do [resilience in .NET](https://learn.microsoft.com/en-us/dotnet/core/resilience/) wich may be suitable to most new projects.  
But this is not compatible with legacy projects.


### Example
```cs
var resilience = new Resilience(5);
var client = new HttpClient();
resilience.ExceptionAction = (i,ex) => Console.WriteLine($"{i}: {ex.Message}");

var result = await resilience.Tryhard(async () => await client.GetStringAsync("https://httpbin.io/unstable"));
```

### Timeout and cancellation
CancellationToken is supported and can be used to set a time limit.  
But it will not terminate a running Task, just prevent a new try.
```cs
using var cts = new CancellationTokenSource();
cts.CancelAfter(10000);
var resilience = new Resilience(-1, ct: cts.Token);

var result = await resilience.Tryhard(async () =>
{
    await Task.Delay(500);
    var rng = Random.Shared.Next(0, 100);
    if (rng > 1) throw new Exception($"{rng}");
    return rng;
});
```

### Why
The deprecation of [Microsoft.Extensions.Http.Polly](https://www.nuget.org/packages/Microsoft.Extensions.Http.Polly) is giving me [left-pad](https://en.wikipedia.org/wiki/Npm_left-pad_incident) vibes so I'm doing my own thing that fits my cases exactly.  
With the age of the cloud came the age of unrealiable services, and I dont want to contribute to the cascading failure.  
Also the similarity with a perk in DBD is a coincidence and I'm not calling anyone a tryhard.

### License
MIT