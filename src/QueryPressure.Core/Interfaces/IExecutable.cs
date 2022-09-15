namespace QueryPressure.Core.Interfaces;

public interface IExecutable
{
    Task ExecuteAsync(CancellationToken cancellationToken);
}