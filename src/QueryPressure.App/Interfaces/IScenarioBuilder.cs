using QueryPressure.App.Arguments;
using QueryPressure.Core;

namespace QueryPressure.App.Interfaces;

public interface IScenarioBuilder
{
  Task<QueryExecutor> BuildAsync(ApplicationArguments arguments, CancellationToken cancellationToken);
}