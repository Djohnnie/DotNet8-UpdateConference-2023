using Microsoft.Extensions.DependencyInjection;


Console.WriteLine("Hello, World!");


var serviceCollection = new ServiceCollection();
serviceCollection.AddKeyedSingleton<TimeProvider>(TimeProviderType.Real, TimeProvider.System);
serviceCollection.AddKeyedTransient<TimeProvider, FakeTimeProvider>(TimeProviderType.Fake);
serviceCollection.AddTransient<MyFirstService>();
serviceCollection.AddTransient<MySecondService>();
var serviceProvider = serviceCollection.BuildServiceProvider();

_ = serviceProvider.GetService<MyFirstService>();
_ = serviceProvider.GetService<MySecondService>();


public enum TimeProviderType
{
    Real,
    Fake
}

public class FakeTimeProvider : TimeProvider
{
    public override DateTimeOffset GetUtcNow()
    {
        return new DateTimeOffset(2000, 1, 1, 13, 37, 0, TimeSpan.Zero);
    }
}

public class MyFirstService
{
    public MyFirstService([FromKeyedServices(TimeProviderType.Real)] TimeProvider timeProvider)
    {
        Console.WriteLine($"{timeProvider.GetType()}: {timeProvider.GetLocalNow()}");
    }
}
public class MySecondService
{
    public MySecondService([FromKeyedServices(TimeProviderType.Fake)] TimeProvider timeProvider)
    {
        Console.WriteLine($"{timeProvider.GetType()}: {timeProvider.GetLocalNow()}");
    }
}