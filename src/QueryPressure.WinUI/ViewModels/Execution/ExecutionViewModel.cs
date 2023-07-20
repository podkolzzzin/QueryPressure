using QueryPressure.WinUI.Commands.Execution;
using QueryPressure.WinUI.Common.Commands;
using QueryPressure.WinUI.Models;
using QueryPressure.WinUI.Services.Language;
using QueryPressure.WinUI.Services.Metric;
using QueryPressure.WinUI.Services.Subscriptions;
using QueryPressure.WinUI.ViewModels.DockElements;
using QueryPressure.WinUI.ViewModels.Helpers.Status;

namespace QueryPressure.WinUI.ViewModels.Execution;

public class ExecutionViewModel : DocumentViewModel
{
  private readonly IDisposable _subscription;
  private readonly ILanguageService _languageService;
  private readonly IExecutionStatusProvider _executionStatusProvider;
  private ExecutionModel? _model;
  private ExecutionStatusViewModel _status;
  private DateTime _startTime;
  private DateTime? _endTime;
  private TimeSpan _duration;
  private Timer _durationUpdateTimer;


  public ExecutionViewModel(ISubscriptionManager subscriptionManager, ILanguageService languageService,
    IExecutionStatusProvider executionStatusProvider, IMetricViewModelFactory metricValueViewModelFactory,
    CloseExecutionResultsCommand closeExecutionResultsCommand, ExecutionModel executionModel)
    : base(executionModel)
  {
    DisplayMetrics = new MetricsViewModel(ContentId, metricValueViewModelFactory);

    _languageService = languageService;
    _executionStatusProvider = executionStatusProvider;
    _status = _executionStatusProvider.GetStatus(ExecutionStatus.None);

    OnExecutionChanged(null, executionModel);

    _subscription = subscriptionManager
      .On(ModelAction.Edit, executionModel)
      .Subscribe(OnExecutionChanged);

    CloseCommand = new DelegateCommand(() => closeExecutionResultsCommand.Execute(_model));

    _durationUpdateTimer = new Timer(UpdateDuration, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));
  }

  private void UpdateDuration(object? state)
  {
    if (StartTime == default || EndTime != null)
    {
      return;
    }

    Duration = GetDuration(StartTime, EndTime);
  }

  private void OnExecutionChanged(object? sender, IModel value)
  {
    if (sender == this)
    {
      return;
    }

    _model = (ExecutionModel)value;

    if (!IsEqualTo(_model))
    {
      throw new InvalidOperationException("Content ID has changed");
    }

    var strings = _languageService.GetStrings();
    Title = $"{_model.Title} - {strings["labels.execution.results"]}";
    Status = _executionStatusProvider.GetStatus(_model.Status);
    StartTime = _model.StartTime;
    EndTime = _model.EndTime == default ? null : _model.EndTime;
    Duration = GetDuration(StartTime, EndTime);


    var displayResultMetrics = _model.ResultMetrics is not null;

    DisplayMetrics.UpdateMetrics(displayResultMetrics ? _model.ResultMetrics : _model.RealtimeMetrics,
      displayResultMetrics ? MetricType.Result : MetricType.Realtime);
  }

  private TimeSpan GetDuration(DateTime startTime, DateTime? endTime)
  {
    return endTime == null ? DateTime.UtcNow - startTime : endTime.Value - startTime;
  }

  public MetricsViewModel DisplayMetrics { get; private set; }

  public ExecutionStatusViewModel Status
  {
    get => _status;
    set => SetField(ref _status, value);
  }

  public DateTime StartTime
  {
    get => _startTime;
    set => SetField(ref _startTime, value);
  }

  public DateTime? EndTime
  {
    get => _endTime;
    set => SetField(ref _endTime, value);
  }

  public TimeSpan Duration
  {
    get => _duration;
    set => SetField(ref _duration, value);
  }

  public override void Dispose()
  {
    _subscription.Dispose();
    _durationUpdateTimer.Dispose();
    base.Dispose();
  }
}
