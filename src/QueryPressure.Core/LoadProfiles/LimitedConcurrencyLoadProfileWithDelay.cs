using QueryPressure.Core.Interfaces;

namespace QueryPressure.Core.LoadProfiles;

public class LimitedConcurrencyLoadProfileWithDelay : IProfile
{
    private record LimitedConcurrencyLoadProfileWithDelayState(int QueryId) : IExecutionDescriptor;
    
    private readonly TimeSpan _delay;
    private readonly IProfile _internal;
    private readonly Task[] _tasks;
    private static readonly Task NeverCompletedTask = new TaskCompletionSource().Task;

    public LimitedConcurrencyLoadProfileWithDelay(int limit, TimeSpan delay)
    {
        _delay = delay;
        _internal = new LimitedConcurrencyLoadProfile(limit);
        _tasks = Enumerable.Range(0, limit).Select(x => Task.CompletedTask).ToArray();
    }
    
    public async Task<IExecutionDescriptor> WhenNextCanBeExecutedAsync(CancellationToken cancellationToken)
    {
        await _internal.WhenNextCanBeExecutedAsync(cancellationToken);
        while (true)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var task = await Task.WhenAny(_tasks);
            var queryId = Array.IndexOf(_tasks, task);
            if (queryId >= 0)
            {
                _tasks[queryId] = NeverCompletedTask;
                return new LimitedConcurrencyLoadProfileWithDelayState(queryId);
            }   
        }
    }

    public async Task OnQueryExecutedAsync(IExecutionDescriptor state, CancellationToken cancellationToken)
    {
        if (state is not LimitedConcurrencyLoadProfileWithDelayState s)
            throw new InvalidOperationException($"The execution state should be {nameof(LimitedConcurrencyLoadProfileWithDelayState)}");

        _tasks[s.QueryId] = Task.Delay(_delay, cancellationToken);
        await _internal.OnQueryExecutedAsync(state, cancellationToken);
    }
}