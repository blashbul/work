# Asp.Net Integration Test avec Claims & Policy Evaluator

```csharp
public class FakePolicyEvaluator : IPolicyEvaluator
{
    private const string AuthenticationScheme = "Test";
    public async Task<AuthenticateResult> AuthenticateAsync(AuthorizationPolicy policy, HttpContext context)
    {
        var principal = new ClaimsPrincipal();
        principal.AddIdentity(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, "652d331d-9b84-43f5-ae03-ffe5ffccdd3d"),
            new Claim("sub","652d331d-9b84-43f5-ae03-ffe5ffccdd3d"),
            new Claim(ClaimTypes.Name, "Test user"),
            new Claim(ClaimTypes.Email, "john-doe@gmail.com"),
            new Claim(ClaimTypes.Role, "ETI_CLIENT")
        }, AuthenticationScheme));

        return await Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(principal,
            new AuthenticationProperties(), AuthenticationScheme)));
    }

    public async Task<PolicyAuthorizationResult> AuthorizeAsync(
        AuthorizationPolicy policy,
        AuthenticateResult authenticationResult,
        HttpContext context, 
        object? resource) 
            => await Task.FromResult(PolicyAuthorizationResult.Success());
}
```

```csharp
public class CustomWebApplicationFactory : 
    WebApplicationFactory<Startup>,
    IAsyncLifetime
{

    public HttpClient HttpClient { get; set; } = default!;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
        => builder
    …
            .ConfigureTestServices(services =>
                services
                    .AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>()
                   …
            );


    public async Task InitializeAsync()
    {
    …

         this.InitiateHttpClient();
    …    
    }

    private void InitiateHttpClient()
    {
        this.HttpClient = this.CreateClient(
            new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

        this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");
    }

    public async Task DisposeAsync()
    {
        this.HttpClient.Dispose();
        …
    }
}
```
