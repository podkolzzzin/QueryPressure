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

    public QueryExecutor(IExecutable executable, IProfile loadProfile, ILimit limit)
    {
        _executable = executable;
        _loadProfile = loadProfile;
        _limit = limit;

        var hooks = ImmutableArray.CreateBuilder<IExecutionHook>();
        
        if (loadProfile is IExecutionHook hookProfile)
            hooks.Add(hookProfile);
        if (limit is IExecutionHook hookLimit)
            hooks.Add(hookLimit);

        _hooks = hooks.ToImmutable();
    }

    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var sw = Stopwatch.StartNew();
        while (!cancellationToken.IsCancellationRequested)
        {
            await _loadProfile.WhenNextCanBeExecutedAsync(cancellationToken);
            var _ = _executable.ExecuteAsync(cancellationToken).ContinueWith(async _ =>
            {
                await Task.WhenAll(_hooks.Select(x => x.OnQueryExecutedAsync(cancellationToken)));
            }, cancellationToken);
            Console.WriteLine(sw.ElapsedMilliseconds);
        }
    }
}