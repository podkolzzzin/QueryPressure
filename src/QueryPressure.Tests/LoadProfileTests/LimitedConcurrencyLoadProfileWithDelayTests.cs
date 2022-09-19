using System;
using System.Threading;
using System.Threading.Tasks;
using QueryPressure.Core.LoadProfiles;
using Xunit;

namespace QueryPressure.Tests.LoadProfileTests;

public class LimitedConcurrencyLoadProfileWithDelayTests
{
    [Fact]
    public async Task WhenNextCanBeExecutedAsync_TheNextTaskAfterLimitExceeded_IsCompleted_OnlyAfterDelay()
    {
        var delay = TimeSpan.FromSeconds(1);
        var profile = new LimitedConcurrencyLoadProfileWithDelay(2, delay);

        var task1 = profile.WhenNextCanBeExecutedAsync(CancellationToken.None);
        var task2 = profile.WhenNextCanBeExecutedAsync(CancellationToken.None);
        var task3 = profile.WhenNextCanBeExecutedAsync(CancellationToken.None);

        await Task.Delay(10);
        
        Assert.True(task1.IsCompletedSuccessfully);
        Assert.True(task2.IsCompletedSuccessfully);
        Assert.False(task3.IsCompleted);

        await profile.OnQueryExecutedAsync(task1.Result, CancellationToken.None);
        await Task.Delay(10);
        Assert.False(task3.IsCompleted);
        await Task.Delay(delay);
        Assert.True(task3.IsCompleted);
    }
}