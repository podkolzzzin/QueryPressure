using QueryPressure.Core.Requirements;

namespace QueryPressure.Core.Interfaces;

public interface IConnectionProvider : ISetting
{
  Task<IServerInfo> GetServerInfoAsync(CancellationToken cancellationToken);
  Task<IExecutable> CreateExecutorAsync(IScriptSource scriptSource, ConnectionRequirement connectionRequirement, CancellationToken cancellationToken);
}

public interface IServerInfo
{
  public string ServerVersion { get; }
}
