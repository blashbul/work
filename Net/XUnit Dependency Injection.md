# XUnit Dependency Injection

<https://beetechnical.com/tech-tutorial/xunit-dependency-injection/>

## Fixture

```csharp
using Microsoft.Extensions.DependencyInjection;
namespace Xunit.Demo.Application.Tests;
public class EmployeeServiceFixture:IDisposable
{
    private readonly IServiceScope _scope;
    public IEmployeeRepository EmployeeRepository;
    
    public EmployeeServiceFixture()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddTransient<IEmployeeRepository, EmployeeRepository>();
        var serviceProvider = serviceCollection.BuildServiceProvider();
        _scope = serviceProvider.CreateScope();
    }

    public void Dispose()
    {
        _scope.Dispose();
    }

    public T GetService<T>()
    {
        return _scope.ServiceProvider.GetRequiredService<T>();
    }
}
```

## Test

```csharp
using Xunit.Demo.Application;
namespace Xunit.Demo.Application.Tests
{
    public class EmployeeServiceTests:IClassFixture<EmployeeServiceFixture>
    {
        private EmployeeService uut;
        public EmployeeServiceTests(EmployeeServiceFixture employeeServiceFixture)
        {
            uut = new EmployeeService(employeeServiceFixture.GetService<IEmployeeRepository>());
        }
        
        [Fact]
        public void AddEmployee_ShouldNotThrow()
        {
            uut.AddEmployee(new Employee()
            {
                Id = 1,
                Age = 34,
                Name = "John"
            });
            Assert.NotEmpty(uut.GetAllEmployees());
        }
    }
}
```
