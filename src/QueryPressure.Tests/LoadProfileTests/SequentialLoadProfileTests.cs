using System.Threading;
using System.Threading.Tasks;
using QueryPressure.Core.Interfaces;
using QueryPressure.Core.LoadProfiles;
using Xunit;

namespace QueryPressure.Tests.LoadProfileTests;

public class SequentialLoadProfileTests
{
  [Fact]
  public void WhenNextCanBeExecutedAsync_FirstCall_ReturnsCompletedTask()
  {
    var profile = new SequentialLoadProfile();
    var task = profile.WhenNextCanBeExecutedAsync(CancellationToken.None);
    Assert.True(task.IsCompletedSuccessfully);
  }

  [Fact]
  public async Task WhenNextCanBeExecutedAsync_SecondCall_CompletesOnlyAfter_WhenQueryExecuted_Called()
  {
    var profile = new SequentialLoadProfile();
    var _ = profile.WhenNextCanBeExecutedAsync(CancellationToken.None);
    var task = profile.WhenNextCanBeExecutedAsync(CancellationToken.None);

    await Task.Delay(10);
    Assert.False(task.IsCompleted);

    await profile.OnQueryExecutedAsync(ExecutionResult.Empty, CancellationToken.None);

    await Task.Delay(10);
    Assert.True(task.IsCompletedSuccessfully);
  }
}
