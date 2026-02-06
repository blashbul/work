# EF Core DbContext Seed on startup

```csharp
using var scope = app.Services.CreateScope();

  var dbContext = scope.ServiceProvider
      .GetRequiredService<ApplicationDbContext>();

// créer la base mais sans les migrations
  dbContext.Database.EnsureCreated();

// créer la base et applique les migrations
  dbContext.Database.Migrate();
```
