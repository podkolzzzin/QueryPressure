using QueryPressure.WinUI.Common.Commands;
using QueryPressure.WinUI.ViewModels;
using QueryPressure.WinUI.ViewModels.Properties;

namespace QueryPressure.WinUI.Commands.App;

public class OpenPropertiesCommand : CommandBase
{
  private readonly DockToolsViewModel _dockToolsViewModel;

  public OpenPropertiesCommand(DockToolsViewModel dockToolsViewModel)
  {
    _dockToolsViewModel = dockToolsViewModel;
  }

  public override bool CanExecute(object? parameter) => true;

  protected override void ExecuteInternal(object? parameter)
  {
    var propertiesViewModel = _dockToolsViewModel.Tools.OfType<PropertiesViewModel>().Single();
    propertiesViewModel.IsVisible = true;
    propertiesViewModel.IsActive = true;
  }
}
