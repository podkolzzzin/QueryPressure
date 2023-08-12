using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using QueryPressure.WinUI.ViewModels.ProjectTree;

namespace QueryPressure.WinUI.Views
{
  /// <summary>
  /// Interaction logic for ProjectTreeView.xaml
  /// </summary>
  public partial class ProjectTreeView : UserControl
  {
    public ProjectTreeView()
    {
      InitializeComponent();
    }

    private void TreeView_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
      var viewModel = (ProjectTreeViewModel)DataContext;
      viewModel.SelectedNode(e.NewValue as BaseNodeViewModel);
    }

    private void OnItemDoubleClick(object sender, MouseButtonEventArgs e)
    {
      if (sender is not TreeViewItem treeItem)
      {
        return;
      }

      if (treeItem.DataContext is not BaseNodeViewModel nodeViewModel)
      {
        return;
      }

      nodeViewModel.Click(e, true);

    }
  }
}
