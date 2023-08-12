using QueryPressure.WinUI.ViewModels.Execution.Metrics;
using System.Windows.Controls;

namespace QueryPressure.WinUI.Views;

/// <summary>
/// Interaction logic for HistogramValueView.xaml
/// </summary>
public partial class HistogramValueView : UserControl, IDisposable
{
  public HistogramValueView()
  {
    InitializeComponent();

    DataContextChanged += HistogramValueView_DataContextChanged;
  }

  private void HistogramValueView_DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
  {
    var viewModel = DataContext as HistogramMetricViewModel;
    if (viewModel is null)
    {
      throw new ArgumentNullException(nameof(viewModel));
    }

    viewModel.SetPlot(HistogramPlot);
  }

  public void Dispose()
  {
    DataContextChanged -= HistogramValueView_DataContextChanged;
  }
}
