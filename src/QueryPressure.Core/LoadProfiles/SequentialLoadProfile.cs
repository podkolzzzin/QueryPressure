using QueryPressure.Core.Interfaces;

namespace QueryPressure.Core.LoadProfiles;

public class SequentialLoadProfile : IProfile, IExecutionHook
{
    private TaskCompletionSource? _taskCompletionSource;
    
    public async Task WhenNextCanBeExecutedAsync(CancellationToken cancellationToken)
    {
        if (_taskCompletionSource != null)
            await _taskCompletionSource.Task;

        _taskCompletionSource = new();
    }

    public Task OnQueryExecutedAsync(CancellationToken cancellationToken)
    {
        if (_taskCompletionSource == null)
            throw new InvalidOperationException(
                $"{nameof(OnQueryExecutedAsync)} is called before first {nameof(WhenNextCanBeExecutedAsync)} called.");
        _taskCompletionSource.SetResult();
        
        return Task.CompletedTask;
    }
}