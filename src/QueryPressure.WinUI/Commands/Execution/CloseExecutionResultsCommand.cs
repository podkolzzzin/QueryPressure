using QueryPressure.WinUI.Common.Commands;
using QueryPressure.WinUI.Models;
using QueryPressure.WinUI.ViewModels;

namespace QueryPressure.WinUI.Commands.Execution;

public class CloseExecutionResultsCommand : CommandBase
{
  private readonly DockToolsViewModel _dockToolsViewModel;

  public CloseExecutionResultsCommand(DockToolsViewModel dockToolsViewModel)
  {
    _dockToolsViewModel = dockToolsViewModel;
  }

  public override bool CanExecute(object? parameter) => true;

  protected override void ExecuteInternal(object? parameter)
  {
    if (parameter is not ExecutionModel execution)
    {
      throw new ArgumentNullException(nameof(execution));
    }

    var fileViewModel = _dockToolsViewModel.Files.OfType<ExecutionViewModel>().FirstOrDefault(fm => fm.IsEqualTo(execution));

    if (fileViewModel == null)
    {
      throw new ArgumentNullException(nameof(execution));
    }

    _dockToolsViewModel.Files.Remove(fileViewModel);
    fileViewModel.Dispose();
  }
}
