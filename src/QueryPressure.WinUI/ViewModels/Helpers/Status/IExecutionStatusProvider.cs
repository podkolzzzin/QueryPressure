using QueryPressure.WinUI.Models;

namespace QueryPressure.WinUI.ViewModels.Helpers.Status;

public interface IExecutionStatusProvider
{
  ExecutionStatusViewModel GetStatus(ExecutionStatus status);
}
