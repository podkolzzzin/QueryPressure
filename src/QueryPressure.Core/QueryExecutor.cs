using System.Collections.Immutable;
using System.Diagnostics;
using QueryPressure.Core.Interfaces;

namespace QueryPressure.Core;

public class QueryExecutor
{
    private readonly IExecutable _executable;
    private readonly IProfile _loadProfile;
    private readonly ILimit _limit;
    private readonly ImmutableArray<IExecutionHook> _hooks;
    private readonly ImmutableArray<IMetricProvider> _metrics;
    
    public QueryExecutor(IExecutable executable, IProfile loadProfile, ILimit limit, IMetricProvider[] metricProviders)
    {
        _executable = executable;
        _loadProfile = loadProfile;
        _limit = limit;
        _metrics = ImmutableArray.Create(metricProviders);

        var hooks = ImmutableArray.CreateBuilder<IExecutionHook>();
        
        if (loadProfile is IExecutionHook hookProfile)
            hooks.Add(hookProfile);
        if (limit is IExecutionHook hookLimit)
            hooks.Add(hookLimit);
        hooks.AddRange(metricProviders.OfType<IExecutionHook>());

        _hooks = hooks.ToImmutable();
    }

    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var source = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, _limit.Token);
        var token = source.Token;
        try
        {
            while (!token.IsCancellationRequested)
            {
                await _loadProfile.WhenNextCanBeExecutedAsync(token);

                if (token.IsCancellationRequested)
                {
                    break;
                }

                var queryStartTime = DateTime.Now;
                var stopwatch = Stopwatch.StartNew();
                var _ = _executable.ExecuteAsync(token).ContinueWith(async _ =>
                {
                    if (token.IsCancellationRequested)
                        return;
                    stopwatch.Stop();
                    var queryEndTime = DateTime.Now;
                    var result = new ExecutionResult(queryStartTime, queryEndTime, stopwatch.Elapsed);
                    await Task.WhenAll(_hooks.Select(x => x.OnQueryExecutedAsync(result, token)));
                }, token);
            }
        }
        catch (OperationCanceledException) {}

        foreach (var metric in _metrics)
        {
            metric.PrintResult();
        }
    }
}