using FluentAssertions;
using Moq;
using System;

namespace DotNet8.TimeAbstraction.Tests;

public class UnitTest1
{

    [Fact]
    public void Test1()
    {
        // Arrange
        var timeProviderMock = new Mock<TimeProvider>();
        timeProviderMock.Setup(m => m.GetUtcNow()).Returns(new DateTimeOffset(2020, 1, 1, 13, 37, 0, TimeSpan.Zero));
        var sut = new MyService(timeProviderMock.Object);

        // Act
        var result = sut.GetCurrentUtc();

        // Assert
        result.Should().Be("01/01/2020 13:37:00 +00:00");
    }

}


public class MyService
{
    private readonly TimeProvider _timeProvider;

    public MyService(TimeProvider timeProvider)
    {
        _timeProvider = timeProvider;
    }

    public string GetCurrentUtc()
    {
        return $"{_timeProvider.GetUtcNow()}";
    }
}