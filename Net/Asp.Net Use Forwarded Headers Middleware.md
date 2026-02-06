# Asp.Net Use Forwarded Headers Middleware

<https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/proxy-load-balancer?view=aspnetcore-8.0>

```csharp
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardLimit = 2;
    options.KnownProxies.Add(IPAddress.Parse("127.0.10.1"));
});

// A Mettre en premier dans les middlewares
app.UseForwardedHeaders();
...

À partir de l’adresse <https://medium.com/@asad99/understanding-known-proxies-in-net-core-75e6f3ad2cb0> 
app.UseForwardedHeaders(newForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto,
    KnownProxies= { IPAddress.Parse("10.130.10.77") },
    ForwardLimit = 2
});

```
