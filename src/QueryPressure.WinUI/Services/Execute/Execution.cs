using Autofac.Features.AttributeFilters;
using QueryPressure.App.Arguments;
using QueryPressure.App.Interfaces;
using QueryPressure.Core.Interfaces;
using QueryPressure.WinUI.Models;
using QueryPressure.WinUI.Services.Subscriptions;

namespace QueryPressure.WinUI.Services.Execute;

public class Execution
{
  private readonly ApplicationArguments _args;
  private readonly IMetricsVisualizer _visualizer;
  private readonly IExecutionResultStore _store;
  private readonly ILiveMetricProvider[] _liveMetricProviders;
  private readonly IScenarioBuilder _builder;
  private readonly IMetricsCalculator _metricsCalculator;
  private readonly ISubscriptionManager _subscriptionManager;
  private readonly ExecutionModel _model;

  public Execution(ApplicationArguments args, Guid id, [KeyFilter(ExecutionVisualizer.Key)] IMetricsVisualizer visualizer, IExecutionResultStore store,
    ILiveMetricProvider[] liveMetricProviders, IScenarioBuilder builder, IMetricsCalculator metricsCalculator,
    ISubscriptionManager subscriptionManager, ExecutionModel model)
  {
    Id = id;

    _args = args;
    _visualizer = visualizer;
    _store = store;
    _liveMetricProviders = liveMetricProviders;
    _builder = builder;
    _metricsCalculator = metricsCalculator;
    _subscriptionManager = subscriptionManager;
    _model = model;
  }

  public Guid Id { get; }

  public async Task ExecuteAsync(CancellationToken cancellationToken)
  {
    NotifyStarted();

    try
    {
      var executor = await _builder.BuildAsync(_args, _store, _liveMetricProviders, default);
      var executeTask = executor.ExecuteAsync(cancellationToken);

      while (!executeTask.IsCompleted)
      {
        await Task.WhenAny(executeTask, Task.Delay(200, cancellationToken));
        var liveVisualization = await _visualizer.VisualizeAsync(_liveMetricProviders.SelectMany(x => x.GetMetrics()), cancellationToken);
        NotifyAsync(liveVisualization);
      }

      if (!executeTask.IsCompletedSuccessfully)
      {
        NotifyError(executeTask.Exception);
        return;
      }

      var errors = _store.Where(x => x.Exception != null).Select(x => x.Exception).ToArray();

      if (errors.Length > 0)
      {
        var firstError = errors.First();

        if (firstError is not null && firstError.InnerException is not null)
        {
          firstError = firstError.InnerException;
        }

        NotifyError(new Exception($"There was {errors.Length} erros during execution. For example: {firstError?.Message}"));
        return;
      }

      var metrics = await _metricsCalculator.CalculateAsync(_store, cancellationToken);
      var visualization = await _visualizer.VisualizeAsync(metrics, cancellationToken);

      NotifyFinnished(_store, visualization);
    }
    catch (Exception ex)
    {
      NotifyError(ex);
    }
  }

  private void NotifyError(Exception? exception)
  {
    _model.Status = ExecutionStatus.Error;
    _model.EndTime = DateTime.UtcNow;
    _model.Exception = exception;
    _subscriptionManager.Notify(this, ModelAction.Edit, _model);
  }

  private void NotifyStarted()
  {
    _model.Status = ExecutionStatus.Running;
    _model.StartTime = DateTime.UtcNow;
    _subscriptionManager.Notify(this, ModelAction.Edit, _model);
  }

  private void NotifyFinnished(IExecutionResultStore store, IVisualization liveVisualization)
  {
    _model.Status = ExecutionStatus.Finished;
    _model.ResultMetrics = (ExecutionVisualization)liveVisualization;
    _model.EndTime = DateTime.UtcNow;
    _model.RowResults = store;
    _subscriptionManager.Notify(this, ModelAction.Edit, _model);
  }

  private void NotifyAsync(IVisualization liveVisualization)
  {
    _model.RealtimeMetrics = (ExecutionVisualization)liveVisualization;
    _subscriptionManager.Notify(this, ModelAction.Edit, _model);
  }
}
