using QueryPressure.WinUI.Commands.App;
using QueryPressure.WinUI.Models;
using QueryPressure.WinUI.ViewModels;
using QueryPressure.WinUI.ViewModels.Execution;

namespace QueryPressure.WinUI.Commands.Execution;

public class CloseExecutionResultsCommand : CloseDocumentCommand<ExecutionModel, ExecutionViewModel>
{
  public CloseExecutionResultsCommand(DockToolsViewModel dockToolsViewModel) : base(dockToolsViewModel)
  {
  }
}
