namespace QueryPressure.Core.Interfaces;

public interface IExecutionHook
{
  Task OnQueryExecutedAsync(CancellationToken cancellationToken);
}