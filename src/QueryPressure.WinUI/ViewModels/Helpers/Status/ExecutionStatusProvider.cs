using QueryPressure.WinUI.Models;

namespace QueryPressure.WinUI.ViewModels.Helpers.Status;

public class ExecutionStatusProvider : IExecutionStatusProvider
{
  private Dictionary<ExecutionStatus, ExecutionStatusViewModel> _statusMapper;

  public ExecutionStatusProvider()
  {
    _statusMapper = new Dictionary<ExecutionStatus, ExecutionStatusViewModel>()
    {
      {ExecutionStatus.None, new  ExecutionStatusViewModel(ExecutionStatus.None, "labels.execution.status.none", new SimpleIcon("none"))},
      {ExecutionStatus.Running, new  ExecutionStatusViewModel(ExecutionStatus.Running, "labels.execution.status.running", new RunningStatusIcon())},
      {ExecutionStatus.Finished, new  ExecutionStatusViewModel(ExecutionStatus.Finished, "labels.execution.status.finished", new SimpleIcon("finished"))},
      {ExecutionStatus.Error, new  ExecutionStatusViewModel(ExecutionStatus.Error, "labels.execution.status.error", new SimpleIcon("error"))},
    };
  }

  public ExecutionStatusViewModel GetStatus(ExecutionStatus status)
  {
    return _statusMapper[status];
  }
}
