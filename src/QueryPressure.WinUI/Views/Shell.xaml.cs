using System.ComponentModel;
using System.Windows;
using QueryPressure.WinUI.ViewModels;

namespace QueryPressure.WinUI.Views;

public partial class Shell : Window
{

  public Shell(ShellViewModel viewModel)
  {
    InitializeComponent();

    DataContext = ViewModel = viewModel;
  }

  public ShellViewModel ViewModel { get; }

  protected override void OnSourceInitialized(EventArgs e)
  {
    base.OnSourceInitialized(e);
    ViewModel.SetWindowPosition(this);
  }

  protected override void OnClosing(CancelEventArgs e)
  {
    base.OnClosing(e);
    ViewModel.SaveWindowPosition(this);
  }
}
