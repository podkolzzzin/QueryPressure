using System;
using System.Threading;
using System.Threading.Tasks;
using QueryPressure.Core.Interfaces;
using QueryPressure.Core.LoadProfiles;
using Xunit;

namespace QueryPressure.Tests.LoadProfileTests;

public class LimitedConcurrencyLoadProfileWithDelayTests
{
  [Fact]
  public async Task WhenNextCanBeExecutedAsync_TheNextAfterLimitExceededTaskCompleted_After_OnQueryExecutedAsync_Called()
  {
    TimeSpan delay = TimeSpan.FromMilliseconds(500);

    var profile = new LimitedConcurrencyWithDelayLoadProfile(2, delay);
    _ = profile.WhenNextCanBeExecutedAsync(CancellationToken.None);
    _ = profile.WhenNextCanBeExecutedAsync(CancellationToken.None);

    var tasks = new[] {
            profile.WhenNextCanBeExecutedAsync(CancellationToken.None),
            profile.WhenNextCanBeExecutedAsync(CancellationToken.None)
        };

    await Task.Delay(15);
    Assert.All(tasks, t => Assert.False(t.IsCompleted));

    _ = profile.OnQueryExecutedAsync(ExecutionResult.Empty, CancellationToken.None);
    _ = profile.OnQueryExecutedAsync(ExecutionResult.Empty, CancellationToken.None);
    Assert.All(tasks, t => Assert.False(t.IsCompleted));

    await Task.Delay(1.5 * delay);
    Assert.All(tasks, t => Assert.True(t.IsCompletedSuccessfully));
  }

  [Fact]
  public async Task WhenNextCanBeExecutedAsync_DelayTasksFinished_AfterCallWhenNextCanBeExecutedAsync()
  {
    TimeSpan delay = TimeSpan.FromMilliseconds(500);

    var profile = new LimitedConcurrencyWithDelayLoadProfile(2, delay);
    _ = profile.WhenNextCanBeExecutedAsync(CancellationToken.None);
    _ = profile.WhenNextCanBeExecutedAsync(CancellationToken.None);

    var tasks = new[] {
            profile.WhenNextCanBeExecutedAsync(CancellationToken.None),
            profile.WhenNextCanBeExecutedAsync(CancellationToken.None),
        };

    await Task.Delay(15);
    Assert.All(tasks, t => Assert.False(t.IsCompleted));

    int startTimestamp = Environment.TickCount;

    _ = profile.OnQueryExecutedAsync(ExecutionResult.Empty, CancellationToken.None);
    _ = profile.OnQueryExecutedAsync(ExecutionResult.Empty, CancellationToken.None);

    await Task.WhenAll(tasks);

    int elapsedMiliseconds = Environment.TickCount - startTimestamp;

    Assert.InRange(elapsedMiliseconds,
        low: delay.TotalMilliseconds,
        high: delay.TotalMilliseconds * 2);
  }

  [Fact]
  public void WhenNextCanBeExecutedAsync_DelayTasksNotFinished_WithoutCallWhenNextCanBeExecutedAsync()
  {
    TimeSpan delay = TimeSpan.FromMilliseconds(250);

    var profile = new LimitedConcurrencyWithDelayLoadProfile(2, delay);
    _ = profile.WhenNextCanBeExecutedAsync(CancellationToken.None);
    _ = profile.WhenNextCanBeExecutedAsync(CancellationToken.None);

    var task = profile.WhenNextCanBeExecutedAsync(CancellationToken.None);
    Assert.False(task.Wait(delay * 4));

    _ = profile.OnQueryExecutedAsync(ExecutionResult.Empty, CancellationToken.None);
    Assert.True(task.Wait(delay * 2));
  }
}
