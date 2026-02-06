# Asp.Net Jwt Authentication

<https://learn.microsoft.com/en-us/aspnet/core/security/authentication/?view=aspnetcore-8.0>
<https://docs.duendesoftware.com/identityserver/v7/apis/aspnetcore/jwt/>

## Startup

```csharp
using Microsoft.AspNetCore.Authentication.JwtBearer;

 var authOptions = new AuthOptions();
 configuration.GetSection(AuthOptions.Section).Bind(authOptions);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
   // if no Resource Api/ scopes 
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.Authority = authOptions.Authority;
    options.TokenValidationParameters.ValidateAudience = false;
});
builder.Services.AddAuthorization();
...
app.UseAuthentication();
app.UseAuthorization();

```

## Class for options

```csharp
public class AuthOptions
{
    /// <summary>
    /// The name of the configuration section.
    /// </summary>
    public const string Section = "Auth";

    /// <summary>
    /// OAuth authority.
    /// </summary>
    public string Authority { get; set; } = null!;

    /// <summary>
    /// OAuth audience.
    /// </summary>
    public string Audience { get; set; } = null!;
}
```

## App settings

```json
{
  ...,
  "Auth": {
    "Authority": "https://identity-test.groupe-sterne.com",
    },
  ...
}

```
