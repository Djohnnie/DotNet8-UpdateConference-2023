using System;


Console.WriteLine("Hello, World!");
var timeProvider = new MockedTimeProvider();


Console.WriteLine(TimeProvider.System.GetUtcNow());
Console.WriteLine(TimeProvider.System.GetLocalNow());


await Task.Delay(TimeSpan.FromMinutes(1), timeProvider);

Console.WriteLine("WAITED !!!");



public class MockedTimeProvider : TimeProvider
{
    public override ITimer CreateTimer(TimerCallback callback, object? state, TimeSpan dueTime, TimeSpan period)
    {
        return base.CreateTimer(callback, state, TimeSpan.Zero, period);
    }
}