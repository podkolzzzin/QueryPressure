namespace QueryPressure.Core.Interfaces;

public record QueryInformation(Guid Id, DateTime QueryStartTime);

public record ExecutionResult(QueryInformation Information, TimeSpan Duration, Exception? Exception)
{
  public static ExecutionResult Empty { get; } = new(new(default, default), default, default);
}

public interface IExecutionHook
{
  Task OnBeforeQueryExecutionAsync(QueryInformation information, CancellationToken cancellationToken) => Task.CompletedTask;
  Task OnQueryExecutedAsync(ExecutionResult result, CancellationToken cancellationToken);
}
