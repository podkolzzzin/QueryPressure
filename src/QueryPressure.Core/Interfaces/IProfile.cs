namespace QueryPressure.Core.Interfaces;

public interface IProfile
{
    Task WhenNextCanBeExecutedAsync(CancellationToken cancellationToken);
    Task OnQueryExecutedAsync(CancellationToken cancellationToken);
}