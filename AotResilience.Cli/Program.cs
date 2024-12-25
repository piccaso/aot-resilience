using AotResilience;

var resilience = new Resilience(5);
var client = new HttpClient();
resilience.ExceptionAction = (i,ex) => Console.WriteLine($"{i}: {ex.Message}");

var result = await resilience.Tryhard(async () => await client.GetStringAsync("https://httpbin.io/unstable"));

Console.WriteLine($"result = {result.Length}");