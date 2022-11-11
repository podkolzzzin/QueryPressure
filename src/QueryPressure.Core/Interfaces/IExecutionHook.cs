namespace QueryPressure.Core.Interfaces;

public record ExecutionResult(DateTime QueryStartTime, DateTime QueryEndTime, TimeSpan Duration)
{
  public static ExecutionResult Empty { get; } = new(default, default, default);
}

public interface IExecutionHook
{
  Task OnQueryExecutedAsync(ExecutionResult result, CancellationToken cancellationToken);
}