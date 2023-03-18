using System;
using System.Threading;
using System.Threading.Tasks;
using QueryPressure.Core.Interfaces;
using QueryPressure.Core.LoadProfiles;
using Xunit;

namespace QueryPressure.Tests.LoadProfileTests;

/*
 * while (true) 
 * {
 *     _executor.ExecuteAsync();
 *     await Task.Delay(delay);
 * }
 */

public class SequentialWithDelayLoadProfileTests
{
  [Fact]
  public void WhenNextCanBeExecutedAsync_FirstCall_ReturnsCompletedTask()
  {
    var profile = new SequentialWithDelayLoadProfile(TimeSpan.FromMilliseconds(500));
    var task = profile.WhenNextCanBeExecutedAsync(CancellationToken.None);
    Assert.True(task.IsCompletedSuccessfully);
  }

  [Fact]
  public async Task WhenNextCanBeExecutedAsync_SecondCall_CompletesOnlyAfter_OnQueryExecutedAsyncCalled_AndDelay()
  {
    var profile = new SequentialWithDelayLoadProfile(TimeSpan.FromMilliseconds(10));
    var _ = profile.WhenNextCanBeExecutedAsync(CancellationToken.None);
    var task = profile.WhenNextCanBeExecutedAsync(CancellationToken.None);
    Assert.False(task.IsCompleted);

    await Task.Delay(15);
    Assert.False(task.IsCompleted);

    await profile.OnQueryExecutedAsync(ExecutionResult.Empty, CancellationToken.None);
    Assert.False(task.IsCompleted);

    await Task.Delay(20);
    Assert.True(task.IsCompletedSuccessfully);
  }

}
