# AotResilience
### Simple resilience compatible with AOT compilation
Very simple retry implementation.  
This is not a full featured Library like [Polly](https://www.pollydocs.org/) by design.  
But it is compatible with AOT compilation and legacy projects.  

There is an official way to do [resilience in .NET](https://learn.microsoft.com/en-us/dotnet/core/resilience/) wich may be suitable to most new projects.  
But this is not compatible with legacy projects.


### Example
```cs
var resilience = new Resilience(100, TimeSpan.FromMilliseconds(50), 10)
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
```

### License
MIT