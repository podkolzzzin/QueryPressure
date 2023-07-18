using QueryPressure.WinUI.Common.Commands;
using QueryPressure.WinUI.Models;
using QueryPressure.WinUI.ViewModels;
using QueryPressure.WinUI.ViewModels.DockElements;

namespace QueryPressure.WinUI.Commands.App;

public abstract class CloseDocumentCommand<TModel, TViewModel> : CommandBase
  where TModel : IModel
  where TViewModel : DocumentViewModel
{
  private readonly DockToolsViewModel _dockToolsViewModel;

  public CloseDocumentCommand(DockToolsViewModel dockToolsViewModel)
  {
    _dockToolsViewModel = dockToolsViewModel;
  }

  public override bool CanExecute(object? parameter) => true;

  protected override void ExecuteInternal(object? parameter)
  {
    if (parameter is not TModel model)
    {
      throw new ArgumentNullException(nameof(model));
    }

    var fileViewModel = _dockToolsViewModel.Files.OfType<TViewModel>().FirstOrDefault(fm => fm.IsEqualTo(model));

    if (fileViewModel == null)
    {
      throw new ArgumentNullException(nameof(model));
    }

    _dockToolsViewModel.Files.Remove(fileViewModel);
    fileViewModel.Dispose();
  }
}
