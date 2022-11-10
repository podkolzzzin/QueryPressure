using QueryPressure.Core.Requirements;

namespace QueryPressure.Core.Interfaces;

public interface IConnectionProvider : ISetting
{
  Task<IExecutable> CreateExecutorAsync(IScriptSource scriptSource, ConnectionRequirement connectionRequirement, CancellationToken cancellationToken);
}