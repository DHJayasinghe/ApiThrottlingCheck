using Microsoft.Extensions.DependencyInjection;

Console.WriteLine("Rate Limit Test started");

var serviceProvider = new ServiceCollection().AddHttpClient().BuildServiceProvider();
var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();

var startTimeWatch = System.Diagnostics.Stopwatch.StartNew();
var minimumResponseTimeInMilliseconds = 10000;

async Task<long> SendRequestAsync(string id)
{
    using var client = httpClientFactory.CreateClient();
    client.Timeout = TimeSpan.FromMinutes(10);

    client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.87 Safari/537.36");
    var watch = System.Diagnostics.Stopwatch.StartNew();
    try
    {
        using var tokenSource = new CancellationTokenSource(TimeSpan.FromMinutes(5));
        var response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Get, "https://ebill.sydneywater.com.au/drsclient-srv/document/U2WOHCCXUZTUQD4EHSUWXH5U6XVE2AACCNFCC7GP4EEOIGEJVX5GBVSRKULJXLDI4RL4CTQC2WH3BC5LR7QJ2XLMZUMHE2E366FJRKI=/7844"), tokenSource.Token);
        watch.Stop();
        var statusText = response.IsSuccessStatusCode ? "Success" : "Failed";
        if (!response.IsSuccessStatusCode)
            Console.WriteLine($"API calls: {id}, Status: {statusText}, Elapsed Time: {watch.ElapsedMilliseconds} ms");
    }
    catch (HttpRequestException ex)
    {
        watch.Stop();
        Console.WriteLine($"API calls: {id}, Status: Timeout, Elapsed Time: {watch.ElapsedMilliseconds} ms " + ex.Message);
    }
    catch (Exception ex)
    {
        watch.Stop();
        Console.WriteLine($"API calls: {id}, Status: UnhandledException, Elapsed Time: {watch.ElapsedMilliseconds} ms " + ex.Message);
    }
    finally
    {
        //if (watch.ElapsedMilliseconds > minimumResponseTimeInMilliseconds)
        //{
        //    Console.WriteLine("Total Time elaped: " + startTimeWatch.Elapsed.TotalSeconds + " secs");
        //    Console.WriteLine("--------------------------------------------------------------------------");
        //}
    }

    return watch.ElapsedMilliseconds;
}

//List<int> batchSizes = new() { 5, 10, 15, 20, 25, 30, 5, 10, 15, 20, 25, 30 };
List<int> batchSizes = new() { 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5 };
foreach (var d in batchSizes.Select((value, index) => new { value, index }))
{
    await RunTest(httpClientFactory, d.index, d.value, 10);
}
startTimeWatch.Stop();

Console.WriteLine("Test finished");

Console.ReadLine();

async Task RunTest(IHttpClientFactory? httpClientFactory, int id, int batchSize = 5, int iterations = 5)
{
    //Console.WriteLine("..... Test #{0} started .....", id);
    //int batchSize = 30;
    //int iterations = 5;
    double totalAverageResponse = 0;
    foreach (var iteration in Enumerable.Range(1, iterations))
    {
        //Console.WriteLine("..... iteration #{0} started .....", iteration);
        var tasks = Enumerable.Range(1, batchSize).Select(d => SendRequestAsync($"I{iteration}-#{d}"));
        var result = await Task.WhenAll(tasks);
        double averageResponse = result.Average();
        totalAverageResponse += averageResponse;
        //Console.WriteLine("..... iteration #{0} completed .....", iteration);
    }
    Console.WriteLine("Test #{3} => Batch Size: {0}, Iterations: {1}, Avg Response Time: {2} ms", batchSize, iterations, Math.Round(totalAverageResponse / iterations, 2), id);

    //Console.WriteLine("..... Test #{0} completed .....", id);
    //Console.WriteLine("Average response time: {0} milliseconds", totalAverageResponse / iterations);
}