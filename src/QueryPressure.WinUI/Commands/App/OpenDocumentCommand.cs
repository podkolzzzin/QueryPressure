using Microsoft.Extensions.Logging;
using QueryPressure.WinUI.Common.Commands;
using QueryPressure.WinUI.ViewModels.DockElements;
using QueryPressure.WinUI.ViewModels;
using QueryPressure.WinUI.Models;

namespace QueryPressure.WinUI.Commands.App;

public abstract class OpenDocumentCommand<TModel, TViewModel> : CommandBase<TModel>
  where TModel : IModel
  where TViewModel : DocumentViewModel
{
  private readonly DockToolsViewModel _dockToolsViewModel;

  protected OpenDocumentCommand(ILogger logger, DockToolsViewModel dockToolsViewModel) : base(logger)
  {
    _dockToolsViewModel = dockToolsViewModel;
  }

  protected override void ExecuteInternal(TModel model)
  {
    var viewModel = Open(model);
    viewModel.IsSelected = true;
    viewModel.IsActive = true;
    _dockToolsViewModel.ActiveDocument = viewModel;
  }

  protected TViewModel Open(TModel model)
  {
    var fileViewModel = _dockToolsViewModel.Files.OfType<TViewModel>().FirstOrDefault(fm => fm.IsEqualTo(model));

    if (fileViewModel != null)
    {
      return fileViewModel;
    }

    fileViewModel = CreateViewModel(model);
    _dockToolsViewModel.Files.Add(fileViewModel);
    return fileViewModel;
  }

  protected abstract TViewModel CreateViewModel(TModel model);
}
