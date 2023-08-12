using Perfolizer.Horology;
using Perfolizer.Mathematics.Histograms;
using QueryPressure.WinUI.Common.Observer;
using QueryPressure.WinUI.Services.Language;
using QueryPressure.WinUI.Services.Theme;
using ScottPlot;
using System.Globalization;

namespace QueryPressure.WinUI.ViewModels.Execution.Metrics;

public class HistogramMetricViewModel : MetricViewModel, IDisposable
{
  private record HistogramDataItem(double Position, double Value, string Label);

  private readonly ISubscription? _languageSubscription;
  private readonly ISubscription? _themeSubscription;

  private WpfPlot? _histogramPlot;
  private Histogram? _currentData;
  private LanguageItem _language;

  private ApplicationTheme _currentTheme;

  public HistogramMetricViewModel(IObservableItem<LanguageItem>? languageObserver, IObservableItem<ApplicationTheme>? themeObserver,
    string contentId, string metricName, string nameLabelKey) : base(nameLabelKey)
  {
    if (languageObserver != null)
    {
      _languageSubscription = languageObserver.SubscribeWithKey(OnLanguageValueChanged, $"{contentId} - {metricName} - {nameof(HistogramMetricViewModel)}");
      _language = languageObserver.CurrentValue;
    }

    if (themeObserver != null)
    {
      _themeSubscription = themeObserver.SubscribeWithKey(OnThemeValueChanged, $"{contentId} - {metricName} - {nameof(HistogramMetricViewModel)}");
      _currentTheme = themeObserver.CurrentValue;
    }
  }

  private void OnThemeValueChanged(object? sender, ApplicationTheme value)
  {
    _currentTheme = value;

    if (_histogramPlot != null && _currentData != null)
    {
      var data = GetHistogramData(_currentData, _language.Locale).ToList();
      UpdatePlot(_histogramPlot, data, _language.Strings[NameLabelKey], _currentTheme);
    }
  }

  private void OnLanguageValueChanged(object? sender, LanguageItem value)
  {
    _language = value;

    if (_histogramPlot != null && _currentData != null)
    {
      var data = GetHistogramData(_currentData, _language.Locale).ToList();
      UpdatePlot(_histogramPlot, data, _language.Strings[NameLabelKey], _currentTheme);
    }
  }

  internal void SetPlot(WpfPlot histogramPlot)
  {
    _histogramPlot = histogramPlot;

    if (_currentData != null)
    {
      var data = GetHistogramData(_currentData, _language.Locale).ToList();
      UpdatePlot(_histogramPlot, data, _language.Strings[NameLabelKey], _currentTheme);
    }
  }

  public override void SetValue(object value)
  {
    var histogram = value as Histogram;

    if (histogram is null)
    {
      throw new ArgumentException(nameof(value));
    }

    _currentData = histogram;

    var data = GetHistogramData(histogram, _language.Locale).ToList();

    if (_histogramPlot is null)
    {
      return;
    }

    UpdatePlot(_histogramPlot, data, _language.Strings[NameLabelKey], _currentTheme);
  }

  private static void UpdatePlot(WpfPlot histogramPlot, IReadOnlyList<HistogramDataItem> histogram, string title, ApplicationTheme theme)
  {
    double[] values = histogram.Select(x => x.Value).ToArray();
    double[] positions = histogram.Select(x => x.Position).ToArray();
    string[] labels = histogram.Select(x => x.Label).ToArray();

    histogramPlot.Configuration.ScrollWheelZoom = false;

    switch (theme)
    {
      case ApplicationTheme.Dark:
        histogramPlot.Plot.Style(Style.Gray2);
        break;
      case ApplicationTheme.Light:
      default:
        histogramPlot.Plot.Style(Style.Default);
        break;
    }

    histogramPlot.Plot.Title(title);
    histogramPlot.Plot.AddBar(values, positions);
    histogramPlot.Plot.XTicks(positions, labels);
    histogramPlot.Plot.XAxis.TickLabelStyle(rotation: 60);
    histogramPlot.Plot.SetAxisLimits(yMin: 0);
    histogramPlot.Refresh();
  }

  private static IEnumerable<HistogramDataItem> GetHistogramData(Histogram histogram, string locale)
  {
    var cultureInfo = CultureInfo.GetCultureInfo(locale);

    string[] array = new string[histogram.Bins.Length];
    string[] array2 = new string[histogram.Bins.Length];
    for (int i = 0; i < histogram.Bins.Length; i++)
    {
      array[i] = FormatValue(histogram.Bins[i].Lower, cultureInfo);
      array2[i] = FormatValue(histogram.Bins[i].Upper, cultureInfo);
    }

    int totalWidth = array.Max((string it) => it.Length);
    int totalWidth2 = array2.Max((string it) => it.Length);

    for (int j = 0; j < histogram.Bins.Length; j++)
    {
      var label = "[" + array[j].PadLeft(totalWidth) + " ; " + array2[j].PadLeft(totalWidth2) + ")";
      var value = histogram.Bins[j].Count;

      yield return new HistogramDataItem(j, value, label);
    }
  }

  private static string FormatValue(double lower, CultureInfo cultureInfo)
  {
    var timeInterval = TimeInterval.FromNanoseconds(lower);
    return timeInterval.ToString(cultureInfo);
  }

  public void Dispose()
  {
    _themeSubscription?.Dispose();
    _languageSubscription?.Dispose();
  }
}
