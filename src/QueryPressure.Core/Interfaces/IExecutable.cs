namespace QueryPressure.Core.Interfaces;

public interface IExecutable : IDisposable
{
  Task ExecuteAsync(CancellationToken cancellationToken);
}
