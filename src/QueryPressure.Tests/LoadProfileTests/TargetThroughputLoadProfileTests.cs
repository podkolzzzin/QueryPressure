using System.Threading;
using System.Threading.Tasks;
using QueryPressure.Core.LoadProfiles;
using Xunit;

namespace QueryPressure.Tests.LoadProfileTests;

public class TargetThroughputLoadProfileTests
{
  [Fact]
  public Task WhenNextCanBeExecutedAsync_FirstCall_Returns_CompletedTask()
  {
    var profile = new TargetThroughputLoadProfile(10);
    var task = profile.WhenNextCanBeExecutedAsync(CancellationToken.None);
    Assert.True(task.IsCompleted);
    return Task.CompletedTask;
  }

  [Fact]
  public async Task WhenNextCanBeExecutedAsync_SecondCall_CompletesTaskOnlyAfterDelay()
  {
    var profile = new TargetThroughputLoadProfile(2);
    var _ = profile.WhenNextCanBeExecutedAsync(CancellationToken.None);
    var task = profile.WhenNextCanBeExecutedAsync(CancellationToken.None);

    await Task.Delay(100);
    Assert.False(task.IsCompleted);

    await Task.Delay(500);
    Assert.True(task.IsCompleted);
  }

  [Fact]
  public async Task WhenNextCanBeExecutedAsync_IfCalledWithoutDelay_CompletionsAreDistributed_InTime()
  {
    var profile = new TargetThroughputLoadProfile(2);
    var _ = profile.WhenNextCanBeExecutedAsync(CancellationToken.None);
    _ = profile.WhenNextCanBeExecutedAsync(CancellationToken.None);
    var task = profile.WhenNextCanBeExecutedAsync(CancellationToken.None);

    await Task.Delay(750);
    Assert.False(task.IsCompleted);

    await Task.Delay(300);
    Assert.True(task.IsCompleted);
  }
}
