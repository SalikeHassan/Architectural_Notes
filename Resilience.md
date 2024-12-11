<img width="986" alt="image" src="https://github.com/user-attachments/assets/9918d00d-aa92-4bb1-98a1-552c7418e53f" />

<img width="973" alt="image" src="https://github.com/user-attachments/assets/0e74b5c2-568f-46bd-bd36-cbfe042fa2b6" />

```csharp
builder.Services.AddHttpClient<StocksApiClient>(client =>
{
    client.BaseAddress = new Uri("http://stocks-api");
})
.AddResilienceHandler("custom", pipeline =>
{
    pipeline.AddCircuitBreaker(new HttpCircuitBreakerStrategyOptions
    {
        SamplingDuration = TimeSpan.FromSeconds(10),
        FailureRatio = 0.5, // Open if 50% of requests fail
        MinimumThroughput = 5, // Minimum 5 requests to evaluate
        BreakDuration = TimeSpan.FromSeconds(30) // Stay open for 30 seconds
    });
});

var fallbackPolicy = Policy<HttpResponseMessage>
    .Handle<Exception>()
    .FallbackAsync(new HttpResponseMessage(HttpStatusCode.OK)
    {
        Content = new StringContent("Fallback response due to service failure.")
    });

var resiliencePolicy = Policy.WrapAsync(fallbackPolicy, retryPolicy, circuitBreakerPolicy);

var response = await resiliencePolicy.ExecuteAsync(() =>
    httpClient.GetAsync("/api/data"));
```
<img width="993" alt="image" src="https://github.com/user-attachments/assets/219e1331-b4d5-4aee-b78d-7fe51d70b783" />

<img width="965" alt="image" src="https://github.com/user-attachments/assets/e7ed9119-a90a-486b-a1af-5cf223dd98f7" />

<img width="884" alt="image" src="https://github.com/user-attachments/assets/c1dae8d1-a039-40eb-b6fe-74edb1bd2ea0" />

https://learn.microsoft.com/en-us/dotnet/core/resilience/http-resilience?tabs=dotnet-cli

```csharp
builder.Services.AddHttpClient<StocksApiClient>(client => client.BaseAddress = new Uri("http://stocks-api"))
    .AddStandardResilienceHandler()
```
