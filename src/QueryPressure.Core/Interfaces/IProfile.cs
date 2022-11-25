namespace QueryPressure.Core.Interfaces;

public interface IProfile : ISetting
{
  Task WhenNextCanBeExecutedAsync(CancellationToken cancellationToken);
}
