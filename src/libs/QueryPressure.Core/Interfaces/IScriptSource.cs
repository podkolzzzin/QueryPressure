namespace QueryPressure.Core.Interfaces;

public interface IScriptSource : ISetting
{
  Task<IScript> GetScriptAsync(CancellationToken cancellationToken);
}
