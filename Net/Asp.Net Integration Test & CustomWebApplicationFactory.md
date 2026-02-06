# Asp.Net Integration Test & CustomWebApplicationFactory

Faire une classe CustomWebApplicationFactory.cs

```csharp
public class CustomWebApplicationFactory : WebApplicationFactory<Startup>, IAsyncLifetime
{
    …

    protected override void ConfigureWebHost(IWebHostBuilder builder)
        => builder
            .UseEnvironment("Docker")
            .ConfigureTestServices(services =>
                services
                    .Add….
                    .Configure<ConnectionOptions>(options => options….)
            );


    public async Task InitializeAsync()
    {
         this.InitiateHttpClient();
    }

    private void InitiateHttpClient()
    {
    ...
    }

    public async Task DisposeAsync()
    {
        this.HttpClient.Dispose();
        …
    }

}

```

**Dans la classe Program.cs si c'est du top level statement ne pas oublier mettre
public partial class Program { }**

```csharp
public class IntegrationTest 
{
    public IntegrationTest(CustomWebApplicationFactory factory)
    {

    }

    [Fact]
    public async Task Get_WhenOK()
    {
        // Arrange

        // Act
        var response = await this.HttpClient.GetAsync(
            new Uri($"uri?param={param=value}", UriKind.Relative),CancellationToken.None);
        var content = await response.Content.ReadAsStringAsync();
    
        // Assert
        response.EnsureSuccessStatusCode();
    
        var returnValues = JsonConvert.DeserializeObject<Model>(content);
        returnValue.Should().Be(test);
    }
```
