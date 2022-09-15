using QueryPressure.Core.Interfaces;

namespace QueryPressure.Core.LoadProfiles;

public class SequentialLoadProfile : IProfile
{
    private TaskCompletionSource? _taskCompletionSource;
    
    public async Task<bool> WhenNextCanBeExecutedAsync(CancellationToken cancellationToken)
    {
        if (_taskCompletionSource != null)
            await _taskCompletionSource.Task;

        _taskCompletionSource = new();
        return !cancellationToken.IsCancellationRequested;
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